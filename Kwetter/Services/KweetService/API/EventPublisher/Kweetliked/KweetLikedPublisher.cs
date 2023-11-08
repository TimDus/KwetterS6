using KweetService.API.EventPublisher.KweetCreated;
using MediatR;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;

namespace KweetService.API.EventPublisher.Kweetliked
{
    public class KweetLikedPublisher : IRequestHandler<KweetLikedEvent>
    {
        private readonly IConnection _connection;

        public KweetLikedPublisher(IConnection connection)
        {
            _connection = connection;
        }

        public async Task Handle(KweetLikedEvent request, CancellationToken cancellationToken)
        {
            var @event = new KweetLikedEvent
            {
            };

            await PublishEvent(@event);
        }

        private async Task<bool> PublishEvent(KweetLikedEvent @event)
        {
            var channel = _connection.CreateModel();
            var exchangeName = "kweet-liked-exchange";
            var routingKey = "kweet.liked";
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event));
            channel.BasicPublish(exchangeName, routingKey, null, body);

            return true;
        }
    }
}
