using KweetService.API.Eventing.EventPublisher.KweetCreated;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using KweetService.API.Repositories;
using KweetService.API.Models.Entity;

namespace KweetService.API.Eventing.EventReceiver.CustomerCreated
{
    public class CustomerCreatedConsumer : AsyncEventingBasicConsumer
    {
        IKweetRepository _repository;

        public CustomerCreatedConsumer(IModel model) : base(model)
        {
        }

        public CustomerCreatedConsumer(IModel model, IKweetRepository repository) : base(model)
        { 
            _repository = repository;
        }

        public override async Task HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, ReadOnlyMemory<byte> body)
        {
            var json = Encoding.UTF8.GetString(body.ToArray());
            var customerCreatedEvent = JsonSerializer.Deserialize<CustomerCreatedEvent>(json);

            Console.WriteLine(customerCreatedEvent.CustomerId);

            CustomerEntity customer = new CustomerEntity(customerCreatedEvent.CustomerId);
            _repository.AddCustomer(customer);

            Model.BasicAck(deliveryTag, false);
        }
    }
}
