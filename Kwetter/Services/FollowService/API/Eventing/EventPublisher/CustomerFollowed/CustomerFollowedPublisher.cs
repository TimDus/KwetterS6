using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace FollowService.API.Eventing.EventPublisher.CustomerFollowed
{
    public class CustomerFollowedPublisher : IRequestHandler<CustomerFollowedEvent>
    {
        private readonly IModel _model;

        public CustomerFollowedPublisher(IConnection connection)
        {
            _model = connection.CreateModel();
        }

        public async Task Handle(CustomerFollowedEvent request, CancellationToken cancellationToken)
        {
            await PublishEvent(request);
        }

        private async Task<bool> PublishEvent(CustomerFollowedEvent @event)
        {
            var exchangeName = "customer-followed-exchange";
            var routingKey = "customer.followed";
            var body = await Task.Run(() => Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event)));
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
