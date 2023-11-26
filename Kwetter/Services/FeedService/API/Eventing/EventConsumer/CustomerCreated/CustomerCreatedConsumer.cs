﻿using Common.Interfaces;
using FeedService.API.Repositories;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace FeedService.API.Eventing.EventConsumer.CustomerCreated
{
    public class CustomerCreatedConsumer : IConsumer<CustomerCreatedEvent>, IDisposable
    {
        private readonly IModel _model;
        private readonly IConnection _connection;
        private readonly IServiceProvider _serviceProvider;

        public CustomerCreatedConsumer(IServiceProvider serviceProvider)
        {
            _connection = serviceProvider.GetRequiredService<IConnection>();
            _model = _connection.CreateModel();
            _model.QueueDeclare(_queueName, durable: true, exclusive: false, autoDelete: false);
            _model.ExchangeDeclare("customer-followed-exchange", ExchangeType.Topic, durable: true, autoDelete: false);
            _model.QueueBind(_queueName, "customer-followed-exchange", string.Empty);
            _serviceProvider = serviceProvider;
        }

        const string _queueName = "customer-followed-queue";

        public async Task<CustomerCreatedEvent> ReadMessages()
        {
            using (var scope = _serviceProvider.CreateScope()) // this will use `IServiceScopeFactory` internally
            {
                var context = scope.ServiceProvider.GetService<IFeedRepository>();
            }
            var consumer = new AsyncEventingBasicConsumer(_model);
            consumer.Received += async (ch, ea) =>
            {
                var body = ea.Body.ToArray();
                var text = System.Text.Encoding.UTF8.GetString(body);
                Console.WriteLine(text);
                await Task.CompletedTask;
                _model.BasicAck(ea.DeliveryTag, false);
            };
            _model.BasicConsume(_queueName, false, consumer);
            await Task.CompletedTask;
            return new CustomerCreatedEvent();
        }

        public void Dispose()
        {
            if (_model.IsOpen)
                _model.Close();
            if (_connection.IsOpen)
                _connection.Close();
        }
    }
}
