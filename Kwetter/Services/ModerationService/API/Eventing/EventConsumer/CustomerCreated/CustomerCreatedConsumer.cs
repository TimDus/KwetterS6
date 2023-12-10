using Common.Interfaces;
using ModerationService.API.Repositories;
using ModerationService.API.Models.Entity;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace ModerationService.API.Eventing.EventConsumer.CustomerCreated
{
    public class CustomerCreatedConsumer : IConsumer<CustomerCreatedEvent>, IDisposable
    {
        private readonly IModel _model;
        private readonly IServiceProvider _serviceProvider;
        const string _queueName = "moderation-customer-created-queue";

        public CustomerCreatedConsumer(IServiceProvider serviceProvider)
        {
            _model = serviceProvider.GetRequiredService<IConnection>().CreateModel();
            _model.QueueDeclare(_queueName, durable: true, exclusive: false, autoDelete: false);
            _model.ExchangeDeclare("customer-created-exchange", ExchangeType.Topic, durable: true, autoDelete: false);
            _model.QueueBind(_queueName, "customer-created-exchange", string.Empty);
            _serviceProvider = serviceProvider;
        }

        public async Task<CustomerCreatedEvent> ReadMessages()
        {
            CustomerCreatedEvent customerCreatedEvent = new();
            var consumer = new AsyncEventingBasicConsumer(_model);
            consumer.Received += async (ch, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body.ToArray());
                customerCreatedEvent = JsonSerializer.Deserialize<CustomerCreatedEvent>(json);

                CustomerEntity customer = new(customerCreatedEvent.CustomerId, customerCreatedEvent.DisplayName, customerCreatedEvent.CustomerName);

                using (var scope = _serviceProvider.CreateScope()) // this will use `IServiceScopeFactory` internally
                {
                    var _repository = scope.ServiceProvider.GetService<IModerationRepository>();

                    await _repository.CreateCustomer(customer);
                }
                _model.BasicAck(ea.DeliveryTag, false);
            };
            _model.BasicConsume(_queueName, false, consumer);
            await Task.CompletedTask;
            return customerCreatedEvent;
        }

        public void Dispose()
        {
            if (_model.IsOpen)
                _model.Close();
        }
    }
}
