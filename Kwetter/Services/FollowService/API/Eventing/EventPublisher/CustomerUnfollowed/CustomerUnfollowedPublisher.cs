using MediatR;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace FollowService.API.Eventing.EventPublisher.CustomerUnfollowed
{
    public class CustomerUnfollowedPublisher : IRequestHandler<CustomerUnfollowedEvent>
    {
        private readonly IModel _model;

        public CustomerUnfollowedPublisher(IConnection connection)
        {
            _model = connection.CreateModel();
        }

        public async Task Handle(CustomerUnfollowedEvent request, CancellationToken cancellationToken)
        {
            await PublishEvent(request);
        }

        private async Task<bool> PublishEvent(CustomerUnfollowedEvent @event)
        {
            var exchangeName = "customer-unfollowed-exchange";
            var routingKey = "customer.unfollowed";
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event));
            _model.BasicPublish(exchangeName, routingKey, null, body);
            await Task.CompletedTask;
            return true;
        }

        public void Dispose()
        {
            if (_model.IsOpen)
                _model.Close();
        }
    }
}
