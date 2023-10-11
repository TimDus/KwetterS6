using Microsoft.AspNetCore.Connections;
using Polly;
using RabbitMQ.Client;
using System.Net.Sockets;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IConnection>(sp =>
{
    var factory = new ConnectionFactory()
    {
        //HostName = "localhost",
        HostName = "rmq",
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
