using CustomerService.API.Eventing.EventPublisher.CustomerCreated;
using CustomerService.API.Logic;
using CustomerService.API.Repositories;
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
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "CustomerService",
        Description = "CustomerService API for Kwetter",

    });
});

//Dependencies
builder.Services.AddTransient<IRequestHandler<CustomerCreatedEvent>, CustomerCreatedPublisher>();
builder.Services.AddTransient<ICustomerLogic, CustomerLogic>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Messaging
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.AddSingleton<IConnection>(sp =>
{
    var factory = new ConnectionFactory()
    {
        VirtualHost = "mnidiotp",
        HostName = "cow-01.rmq2.cloudamqp.com",
        Port = 5672,
        UserName = "mnidiotp",
        Password = "k4l71JcIUK-t-Z2YSdOr1sr27eRCIH8T",
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
    var queueName = "customer-created-queue";
    var exclusive = false;
    var arguments = new Dictionary<string, object>();

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
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapControllers();

app.Run();
