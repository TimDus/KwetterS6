using Common.Interfaces;
using KweetService.API.Eventing.EventConsumer.CustomerCreated;
using KweetService.API.Eventing.EventPublisher.KweetCreated;
using KweetService.API.Eventing.EventPublisher.KweetLiked;
using KweetService.API.Eventing.EventPublisher.KweetUnliked;
using KweetService.API.Eventing.EventReceiver.CustomerCreated;
using KweetService.API.Logic;
using KweetService.API.Repositories;
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
        Title = "KweetService",
        Description = "KweetService API for Kwetter",

    });
});

//Dependencies
builder.Services.AddTransient<IRequestHandler<KweetCreatedEvent>, KweetCreatedPublisher>();
builder.Services.AddTransient<IRequestHandler<KweetLikedEvent>, KweetLikedPublisher>();
builder.Services.AddTransient<IRequestHandler<KweetUnlikedEvent>, KweetUnlikedPublisher>();
builder.Services.AddTransient<IKweetLogic, KweetLogic>();
builder.Services.AddScoped<IKweetRepository, KweetRepository>();

builder.Services.AddSingleton<IConsumer<CustomerCreatedEvent>, CustomerCreatedConsumer>();
builder.Services.AddHostedService<CustomerCreatedHosted>();

//Messaging
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.AddSingleton<IConnection>(sp =>
{
    var factory = new ConnectionFactory();

    if (builder.Environment.IsDevelopment())
    {
        string name = "localhost";
        if(Environment.GetEnvironmentVariable("DOCKER") == "Docker")
        {
            name = "rabbitmq";
        }
        factory = new ConnectionFactory()
        {
            HostName = name,
            Port = 5672,
            UserName = "guest",
            Password = "guest",
            DispatchConsumersAsync = true
        };
    }
    else
    {
        factory = new ConnectionFactory()
        {
            VirtualHost = "mnidiotp",
            HostName = "cow-01.rmq2.cloudamqp.com",
            Port = 5672,
            UserName = "mnidiotp",
            Password = "k4l71JcIUK-t-Z2YSdOr1sr27eRCIH8T",
            DispatchConsumersAsync = true
        };
    }

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

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(8102);
});

//Datbase Context
string dbHost = builder.Configuration.GetValue<string>("Database:DB_HOST");
string dbName = builder.Configuration.GetValue<string>("Database:DB_NAME");
string dbPassword = builder.Configuration.GetValue<string>("Database:DB_PASSWORD");

var connectionString = $"Data Source={dbHost};Initial Catalog={dbName};User ID=sa;Password={dbPassword};TrustServerCertificate=true";

builder.Services.AddDbContext<KweetDbContext>(opt => opt.UseSqlServer(connectionString));

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
    var exchangeName = "kweet-created-exchange";
    var queueName = "moderation-kweet-created-queue";

    channel.ExchangeDeclare(exchangeName, exchangeType, durable, autoDelete);
    channel.QueueDeclare(queueName, durable, exclusive, autoDelete, arguments);
    channel.QueueBind(queueName, exchangeName, "kweet.created");

    queueName = "feed-kweet-created-queue";
    channel.QueueDeclare(queueName, durable, exclusive, autoDelete, arguments);
    channel.QueueBind(queueName, exchangeName, "kweet.created");

    //kweet liked
    exchangeName = "kweet-liked-exchange";
    queueName = "kweet-liked-queue";

    channel.ExchangeDeclare(exchangeName, exchangeType, durable, autoDelete);
    channel.QueueDeclare(queueName, durable, exclusive, autoDelete, arguments);
    channel.QueueBind(queueName, exchangeName, "kweet.liked");

    //kweet unliked
    exchangeName = "kweet-unliked-exchange";
    queueName = "kweet-unliked-queue";

    channel.ExchangeDeclare(exchangeName, exchangeType, durable, autoDelete);
    channel.QueueDeclare(queueName, durable, exclusive, autoDelete, arguments);
    channel.QueueBind(queueName, exchangeName, "kweet.unliked");
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
