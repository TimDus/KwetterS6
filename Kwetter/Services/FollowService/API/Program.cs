using Common.Interfaces;
using Common.Startup;
using FollowService.API.Eventing.EventConsumer.CustomerCreated;
using FollowService.API.Eventing.EventPublisher.CustomerFollowed;
using FollowService.API.Eventing.EventPublisher.CustomerUnfollowed;
using FollowService.API.Logic;
using FollowService.API.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
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

builder.Services.AddSingleton<IConsumer<CustomerCreatedEvent>, CustomerCreatedConsumer>();
builder.Services.AddHostedService<CustomerCreatedHosted>();

//Messaging
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

RabbitMQConnection.SetupRabbitMQConnection(builder.Services, builder.Environment.EnvironmentName);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(8103);
});

//Datbase Context
string dbHost = builder.Configuration.GetValue<string>("Database:DB_HOST");
string dbName = builder.Configuration.GetValue<string>("Database:DB_NAME");
string dbPassword = builder.Configuration.GetValue<string>("Database:DB_PASSWORD");

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
