using FeedService.API.Eventing.EventReceiver.KweetCreated;
using FeedService.API.Logic;
using FeedService.API.Repositories;
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
        Title = "FeedService",
        Description = "FeedService API for Kwetter",

    });
});

//Dependencies
builder.Services.AddTransient<IFeedLogic, FeedLogic>();
builder.Services.AddScoped<IFeedRepository, FeedRepository>();

//Messaging
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

string rmqName;

if (builder.Environment.IsDevelopment())
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

if (builder.Environment.IsDevelopment())
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
    dbPassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
}

var connectionString = $"Data Source={dbHost};Initial Catalog={dbName};User ID=sa;Password={dbPassword};TrustServerCertificate=true";

builder.Services.AddDbContext<FeedDbContext>(opt => opt.UseSqlServer(connectionString));

var app = builder.Build();

//rabbitmq channels
using (var scope = app.Services.CreateScope())
{
    // Declare the exchange
    var connection = scope.ServiceProvider.GetRequiredService<IConnection>();
    var channel = connection.CreateModel();
    var exchangeName = "kweet-created-exchange";
    var exchangeType = ExchangeType.Topic;
    var durable = true;
    var autoDelete = false;

    channel.ExchangeDeclare(exchangeName, exchangeType, durable, autoDelete);

    // Declare a queue and bind it to the exchange
    var queueName = "kweet-created-queue";
    var exclusive = false;
    var arguments = new Dictionary<string, object>();

    channel.QueueDeclare(queueName, durable, exclusive, autoDelete, arguments);
    channel.QueueBind(queueName, exchangeName, "kweet.created");

    // Declare consumer
    var consumer = new KweetCreatedConsumer(channel);
    channel.BasicConsume(queueName, autoAck: false, consumer);
}

// Configure the HTTP request pipeline.
app.UseSwagger(c =>
{
    c.SerializeAsV2 = true;
});
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "KweetApi V1");
});

app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapControllers();

app.Run();
