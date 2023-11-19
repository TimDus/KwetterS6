using MediatR;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;

namespace FollowService.API.Eventing.EventPublisher.CustomerUnfollowed
{
    public class CustomerUnfollowedPublisher : IRequestHandler<CustomerUnfollowedEvent>
    {
        private readonly IConnection _connection;

        public CustomerUnfollowedPublisher(IConnection connection)
        {
            _connection = connection;
        }

        public async Task Handle(CustomerUnfollowedEvent request, CancellationToken cancellationToken)
        {
            await PublishEvent(request);
        }

        private async Task<bool> PublishEvent(CustomerUnfollowedEvent @event)
        {
            var channel = _connection.CreateModel();
            var exchangeName = "customer-unfollowed-exchange";
            var routingKey = "customer.unfollowed";
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event));
            channel.BasicPublish(exchangeName, routingKey, null, body);

            return true;
        }
    }
}
