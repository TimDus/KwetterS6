using MediatR;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace CustomerService.API.Eventing.EventPublisher.CustomerCreated
{
    public class CustomerCreatedPublisher : IRequestHandler<CustomerCreatedEvent>
    {
        private readonly IConnection _connection;

        public CustomerCreatedPublisher(IConnection connection)
        {
            _connection = connection;
        }

        public async Task Handle(CustomerCreatedEvent request, CancellationToken cancellationToken)
        {
            await PublishEvent(request);
        }

        private async Task<bool> PublishEvent(CustomerCreatedEvent @event)
        {
            var channel = _connection.CreateModel();
            var exchangeName = "customer-created-exchange";
            var routingKey = "customer.created";
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event));
            channel.BasicPublish(exchangeName, routingKey, null, body);

            return true;
        }
    }
}
