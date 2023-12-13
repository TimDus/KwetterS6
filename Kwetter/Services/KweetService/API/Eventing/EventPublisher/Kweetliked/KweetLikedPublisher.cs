using MediatR;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace KweetService.API.Eventing.EventPublisher.KweetLiked
{
    public class KweetLikedPublisher : IRequestHandler<KweetLikedEvent>
    {
        private readonly IConnection _connection;

        public KweetLikedPublisher(IConnection connection)
        {
            _connection = connection;
        }

        public async Task Handle(KweetLikedEvent @event, CancellationToken cancellationToken)
        {
            await PublishEvent(@event);
        }

        private async Task<bool> PublishEvent(KweetLikedEvent @event)
        {
            var exchangeName = "kweet-liked-exchange";
            var routingKey = "kweet.liked";
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event));
            using (var channel = _connection.CreateModel())
            {
                channel.BasicPublish(exchangeName, routingKey, null, body);
            }
            await Task.CompletedTask;
            return true;
        }
    }
}
