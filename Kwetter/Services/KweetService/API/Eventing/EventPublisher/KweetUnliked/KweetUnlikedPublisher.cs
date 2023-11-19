using MediatR;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;

namespace KweetService.API.Eventing.EventPublisher.KweetUnliked
{
    public class KweetUnlikedPublisher : IRequestHandler<KweetUnlikedEvent>
    {
        private readonly IConnection _connection;

        public KweetUnlikedPublisher(IConnection connection)
        {
            _connection = connection;
        }

        public async Task Handle(KweetUnlikedEvent @event, CancellationToken cancellationToken)
        {
            await PublishEvent(@event);
        }

        private async Task<bool> PublishEvent(KweetUnlikedEvent @event)
        {
            var channel = _connection.CreateModel();
            var exchangeName = "kweet-unliked-exchange";
            var routingKey = "kweet.unliked";
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event));
            channel.BasicPublish(exchangeName, routingKey, null, body);

            return true;
        }
    }
}
