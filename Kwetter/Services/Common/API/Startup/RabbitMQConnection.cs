using Microsoft.Extensions.DependencyInjection;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;

namespace Common.Startup
{
    public static class RabbitMQConnection
    {
        public static void SetupRabbitMQConnection(this IServiceCollection services, string env)
        {
            services.AddSingleton<IConnection>(sp =>
            {
                var factory = new ConnectionFactory();

                if (env == "Development" || Environment.GetEnvironmentVariable("DOCKER") == "Docker")
                {
                    //string name = "localhost";
                    //if (Environment.GetEnvironmentVariable("DOCKER") == "Docker")
                    //{
                    //    name = "rabbitmq";
                    //}
                    //factory = new ConnectionFactory()
                    //{
                    //    HostName = name,
                    //    Port = 5672,
                    //    UserName = "guest",
                    //    Password = "guest",
                    //    DispatchConsumersAsync = true
                    //};
                    factory = new ConnectionFactory()
                    {
                        VirtualHost = "mnidiotp",
                        HostName = "cow-01.rmq2.cloudamqp.com",
                        Port = 5672,
                        UserName = "mnidiotp",
                        Password = "Hki1eA6NFnm9ncwwToOjGhX8b-97TSXN",
                        DispatchConsumersAsync = true
                    };
                }
                else
                {
                    //factory = new ConnectionFactory()
                    //{
                    //    HostName = "rabbitmq-service",
                    //    Port = 5672,
                    //    UserName = "guest",
                    //    Password = "guest",
                    //    DispatchConsumersAsync = true
                    //};
                    //cloud connection
                    factory = new ConnectionFactory()
                    {
                        VirtualHost = "mnidiotp",
                        HostName = "cow-01.rmq2.cloudamqp.com",
                        Port = 5672,
                        UserName = "mnidiotp",
                        Password = "Hki1eA6NFnm9ncwwToOjGhX8b-97TSXN",
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
        }
    }
}
