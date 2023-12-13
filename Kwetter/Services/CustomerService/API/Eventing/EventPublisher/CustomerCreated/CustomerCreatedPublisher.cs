using MediatR;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace CustomerService.API.Eventing.EventPublisher.CustomerCreated
{
    public class CustomerCreatedPublisher : IRequestHandler<CustomerCreatedEvent>
    {
        private readonly IModel _model;

        public CustomerCreatedPublisher(IConnection connection)
        {
            _model = connection.CreateModel();
        }

        public async Task Handle(CustomerCreatedEvent request, CancellationToken cancellationToken)
        {
            await PublishEvent(request);
        }

        private async Task<bool> PublishEvent(CustomerCreatedEvent @event)
        {
            var exchangeName = "customer-created-exchange";
            var routingKey = "customer.created";
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event));
            _model.BasicPublish(exchangeName, routingKey, null, body);

            return true;
        }

        public void Dispose()
        {
            if (_model.IsOpen)
                _model.Close();
        }
    }
}
