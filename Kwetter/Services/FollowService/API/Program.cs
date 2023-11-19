using FollowService.API.Eventing.EventPublisher.CustomerFollowed;
using FollowService.API.Eventing.EventPublisher.CustomerUnfollowed;
using FollowService.API.Logic;
using FollowService.API.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

//swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "FollowService",
        Description = "FollowService API for Kwetter",

    });
});

//Dependencies
builder.Services.AddTransient<IRequestHandler<CustomerFollowedEvent>, CustomerFollowedPublisher>();
builder.Services.AddTransient<IRequestHandler<CustomerUnfollowedEvent>, CustomerUnfollowedPublisher>();
builder.Services.AddTransient<IFollowLogic, FollowLogic>();
builder.Services.AddScoped<IFollowRepository, FollowRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Messaging
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

string rmqName;

if (builder.Environment.IsDevelopment() && Environment.GetEnvironmentVariable("DOCKER") != "Docker")
{
    rmqName = "localhost";
}
else
{
    rmqName = "rabbitmq";
}

builder.Services.AddSingleton<IConnection>(sp =>
{
    var factory = new ConnectionFactory()
    {
        HostName = rmqName,
        Port = 5672,
        UserName = "guest",
        Password = "guest",
        DispatchConsumersAsync = true
    };

    var retryPolicy = Policy.Handle<SocketException>()
        .WaitAndRetry(new[]
        {
            TimeSpan.FromSeconds(1),
            TimeSpan.FromSeconds(2),
            TimeSpan.FromSeconds(3)
        });

    return retryPolicy.Execute(() =>
    {
        while (true)
        {
            try
            {
                return factory.CreateConnection();
            }
            catch (Exception ex) when (ex is SocketException || ex is BrokerUnreachableException)
            {
                Console.WriteLine("RabbitMQ Client is trying to connect...");
            }
        }
    });
});

//Datbase Context
string dbHost;
string dbName;
string dbPassword;

if (builder.Environment.IsDevelopment() && Environment.GetEnvironmentVariable("DOCKER") != "Docker")
{
    dbHost = builder.Configuration.GetValue<string>("Database:DB_HOST");
    dbName = builder.Configuration.GetValue<string>("Database:DB_NAME");
    dbPassword = builder.Configuration.GetValue<string>("Database:DB_PASSWORD");
}
else
{
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
    dbHost = Environment.GetEnvironmentVariable("DB_HOST");
    dbName = Environment.GetEnvironmentVariable("DB_NAME");
    dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
}

var connectionString = $"Data Source={dbHost};Initial Catalog={dbName};User ID=sa;Password={dbPassword};TrustServerCertificate=true";

builder.Services.AddDbContext<FollowDBContext>(opt => opt.UseSqlServer(connectionString));

var app = builder.Build();

//rabbitmq channels
using (var scope = app.Services.CreateScope())
{
    //general settings
    var connection = scope.ServiceProvider.GetRequiredService<IConnection>();
    var durable = true;
    var autoDelete = false;
    var channel = connection.CreateModel();
    var exclusive = false;
    var arguments = new Dictionary<string, object>();
    var exchangeType = ExchangeType.Topic;

    //kweet created
    var exchangeName = "customer-followed-exchange";
    var queueName = "customer-followed-queue";

    channel.ExchangeDeclare(exchangeName, exchangeType, durable, autoDelete);
    channel.QueueDeclare(queueName, durable, exclusive, autoDelete, arguments);
    channel.QueueBind(queueName, exchangeName, "customer.followed");

    //kweet liked
    exchangeName = "customer-unfollowed-exchange";
    queueName = "customer-unfollowed-queue";

    channel.ExchangeDeclare(exchangeName, exchangeType, durable, autoDelete);
    channel.QueueDeclare(queueName, durable, exclusive, autoDelete, arguments);
    channel.QueueBind(queueName, exchangeName, "customer.unfollowed");
}

// Configure the HTTP request pipeline.
app.UseSwagger(c =>
{
    c.SerializeAsV2 = true;
});
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "FollowApi V1");
});

app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapControllers();

app.Run();
