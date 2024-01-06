using CustomerService.API.Eventing.EventPublisher.CustomerCreated;
using CustomerService.API.Logic;
using CustomerService.API.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using Swashbuckle.AspNetCore.Filters;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "CustomerService",
        Description = "CustomerService API for Kwetter",

    });
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard auth header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.IncludeErrorDetails = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

//Dependencies
builder.Services.AddTransient<IRequestHandler<CustomerCreatedEvent>, CustomerCreatedPublisher>();
builder.Services.AddTransient<ICustomerLogic, CustomerLogic>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

//Messaging
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.AddSingleton<IConnection>(sp =>
{
    var factory = new ConnectionFactory();

    if (builder.Environment.IsDevelopment())
    {
        string name = "localhost";
        if (Environment.GetEnvironmentVariable("DOCKER") == "Docker")
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
            HostName = "rabbitmq-service",
            Port = 5672,
            UserName = "guest",
            Password = "guest",
            DispatchConsumersAsync = true
        };
        /* cloud connection
        factory = new ConnectionFactory()
        {
            VirtualHost = "mnidiotp",
            HostName = "cow-01.rmq2.cloudamqp.com",
            Port = 5672,
            UserName = "mnidiotp",
            Password = "k4l71JcIUK-t-Z2YSdOr1sr27eRCIH8T",
            DispatchConsumersAsync = true
        };
        */
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
    serverOptions.ListenAnyIP(8101);
});

//Datbase Context
string dbHost = builder.Configuration.GetValue<string>("Database:DB_HOST");
string dbName = builder.Configuration.GetValue<string>("Database:DB_NAME");
string dbPassword = builder.Configuration.GetValue<string>("Database:DB_PASSWORD");

var connectionString = $"Data Source={dbHost};Initial Catalog={dbName};User ID=sa;Password={dbPassword};TrustServerCertificate=true";

builder.Services.AddDbContext<CustomerDBContext>(opt => opt.UseSqlServer(connectionString));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{

    // Declare the exchange
    var connection = scope.ServiceProvider.GetRequiredService<IConnection>();
    var channel = connection.CreateModel();
    var exchangeName = "customer-created-exchange";
    var exchangeType = ExchangeType.Topic;
    var durable = true;
    var autoDelete = false;

    channel.ExchangeDeclare(exchangeName, exchangeType, durable, autoDelete);

    // Declare a queue and bind it to the exchange
    var exclusive = false;
    var arguments = new Dictionary<string, object>();

    var queueName = "kweet-customer-created-queue";
    channel.QueueDeclare(queueName, durable, exclusive, autoDelete, arguments);
    channel.QueueBind(queueName, exchangeName, "customer.created");

    queueName = "feed-customer-created-queue";
    channel.QueueDeclare(queueName, durable, exclusive, autoDelete, arguments);
    channel.QueueBind(queueName, exchangeName, "customer.created");

    queueName = "follow-customer-created-queue";
    channel.QueueDeclare(queueName, durable, exclusive, autoDelete, arguments);
    channel.QueueBind(queueName, exchangeName, "customer.created");

    queueName = "moderation-customer-created-queue";
    channel.QueueDeclare(queueName, durable, exclusive, autoDelete, arguments);
    channel.QueueBind(queueName, exchangeName, "customer.created");
}

// Configure the HTTP request pipeline.
app.UseSwagger(c =>
{
    c.SerializeAsV2 = true;
});
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CustomerAPI V1");
});

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapControllers();

app.Run();
