using Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ModerationService.API.Eventing.EventConsumer.CustomerCreated;
using ModerationService.API.Eventing.EventConsumer.KweetCreated;
using ModerationService.API.Logic;
using ModerationService.API.Repositories;
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
builder.Services.AddSingleton<IConsumer<CustomerCreatedEvent>, CustomerCreatedConsumer>();
builder.Services.AddHostedService<CustomerCreatedHosted>();

builder.Services.AddSingleton<IConsumer<KweetCreatedEvent>, KweetCreatedConsumer>();
builder.Services.AddHostedService<KweetCreatedHosted>();

builder.Services.AddTransient<IModerationLogic, ModerationLogic>();
builder.Services.AddScoped<IModerationRepository, ModerationRepository>();

//Messaging
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.AddSingleton<IConnection>(sp =>
{
    var factory = new ConnectionFactory();

    if (builder.Environment.IsDevelopment() && Environment.GetEnvironmentVariable("DOCKER") != "Docker")
    {
        factory = new ConnectionFactory()
        {
            HostName = "localhost",
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
    serverOptions.ListenAnyIP(8105);
});

//Datbase Context
string dbHost = builder.Configuration.GetValue<string>("Database:DB_HOST");
string dbName = builder.Configuration.GetValue<string>("Database:DB_NAME");
string dbPassword = builder.Configuration.GetValue<string>("Database:DB_PASSWORD");

var connectionString = $"Data Source={dbHost};Initial Catalog={dbName};User ID=sa;Password={dbPassword};TrustServerCertificate=true";

builder.Services.AddDbContext<ModerationDbContext>(opt => opt.UseSqlServer(connectionString));

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
    var exchangeName = "kweet-moderated-exchange";
    var queueName = "kweet-moderated-queue";

    channel.ExchangeDeclare(exchangeName, exchangeType, durable, autoDelete);
    channel.QueueDeclare(queueName, durable, exclusive, autoDelete, arguments);
    channel.QueueBind(queueName, exchangeName, "kweet.moderated");
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
