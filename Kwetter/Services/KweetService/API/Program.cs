using KweetService.API.EventPublisher.CreateKweet;
using KweetService.API.EventPublisher.KweetCreated;
using KweetService.API.EventPublisher.Kweetliked;
using KweetService.API.EventPublisher.KweetUnliked;
using KweetService.API.Logic;
using MediatR;
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
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "KweetService",
        Description = "KweetService API for Kwetter",

    });
});

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.AddTransient<IRequestHandler<KweetCreatedEvent>, KweetCreatedPublisher>();
builder.Services.AddTransient<IRequestHandler<KweetLikedEvent>, KweetLikedPublisher>();
builder.Services.AddTransient<IRequestHandler<KweetUnlikedEvent>, KweetUnlikedPublisher>();
builder.Services.AddTransient<IKweetLogic, KweetLogic>();

string rmqName;

if (builder.Environment.IsDevelopment())
{
    rmqName = "localhost";
} else
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

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var connection = scope.ServiceProvider.GetRequiredService<IConnection>();
    var channel = connection.CreateModel();
    var exchangeName = "kweet-created-exchange";
    var exchangeType = ExchangeType.Direct;
    var durable = true;
    var autoDelete = false;

    channel.ExchangeDeclare(exchangeName, exchangeType, durable, autoDelete);
    var queueName = "kweet-created-queue";
    var exclusive = false;
    var arguments = new Dictionary<string, object>();

    channel.QueueDeclare(queueName, durable, exclusive, autoDelete, arguments);
    channel.QueueBind(queueName, exchangeName, "kweet.created");
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
