using Common.Eventing;
using Common.Interfaces;
using FeedService.API.Eventing.EventConsumer.CustomerFollowed;
using FeedService.API.Eventing.EventConsumer.CustomerUnfollowed;
using FeedService.API.Eventing.EventConsumer.KweetCreated;
using FeedService.API.Eventing.EventConsumer.KweetLiked;
using FeedService.API.Logic;
using FeedService.API.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Polly;
using RabbitMQ.Client.Exceptions;
using RabbitMQ.Client;
using System.Net.Sockets;
using System.Reflection;
using FeedService.API.Eventing.EventConsumer.CustomerCreated;

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

builder.Services.AddSingleton<IConsumer<CustomerCreatedEvent>, CustomerCreatedConsumer>();
builder.Services.AddHostedService<CustomerCreatedHosted>();

builder.Services.AddSingleton<IConsumer<KweetCreatedEvent>, KweetCreatedConsumer>();
builder.Services.AddHostedService<KweetCreatedHosted>();

builder.Services.AddSingleton<IConsumer<KweetLikedEvent>, KweetLikedConsumer>();
builder.Services.AddHostedService<KweetLikedHosted>();

builder.Services.AddSingleton<IConsumer<CustomerFollowedEvent>, CustomerFollowedConsumer>();
builder.Services.AddHostedService<CustomerFollowedHosted>();

builder.Services.AddSingleton<IConsumer<CustomerUnfollowedEvent>, CustomerUnfollowedConsumer>();
builder.Services.AddHostedService<CustomerUnfollowedHosted>();

//Messaging
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.Configure<RabbitMqConfiguration>(builder.Configuration.GetSection("RabbitMqConfiguration"));

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

builder.Services.AddDbContext<FeedDbContext>(opt => opt.UseSqlServer(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger(c =>
{
    c.SerializeAsV2 = true;
});
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "FeedApi V1");
});

app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapControllers();

app.Run();
