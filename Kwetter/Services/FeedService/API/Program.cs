using Common.Interfaces;
using Common.Startup;
using FeedService.API.Eventing.EventConsumer.CustomerCreated;
using FeedService.API.Eventing.EventConsumer.CustomerFollowed;
using FeedService.API.Eventing.EventConsumer.CustomerUnfollowed;
using FeedService.API.Eventing.EventConsumer.KweetCreated;
using FeedService.API.Eventing.EventConsumer.KweetLiked;
using FeedService.API.Logic;
using FeedService.API.Repositories;
using FeedService.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
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
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IKweetRepository, KweetRepository>();
builder.Services.AddScoped<IKweetLikeRepository, KweetLikeRepository>();
builder.Services.AddScoped<IFollowRepository, FollowRepository>();

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

RabbitMQConnection.SetupRabbitMQConnection(builder.Services, builder.Environment.EnvironmentName);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(8104);
});

//Database Context
string dbHost = builder.Configuration.GetValue<string>("Database:DB_HOST");
string dbName = builder.Configuration.GetValue<string>("Database:DB_NAME");
string dbPassword = builder.Configuration.GetValue<string>("Database:DB_PASSWORD");

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
