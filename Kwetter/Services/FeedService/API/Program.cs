using Common.Eventing;
using Common.Interfaces;
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

builder.Services.AddSingleton<IConsumerSetup, ConsumerSetup>();
builder.Services.AddSingleton<IConsumer<KweetCreatedEvent>, KweetCreatedConsumer>();
builder.Services.AddHostedService<KweetCreatedHosted>();

//Messaging
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.Configure<RabbitMqConfiguration>(builder.Configuration.GetSection("RabbitMqConfiguration"));

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
