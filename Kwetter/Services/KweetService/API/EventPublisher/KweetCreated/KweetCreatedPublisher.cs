using Common;
using MediatR;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using KweetService.API.EventPublisher.KweetCreated;

namespace KweetService.API.EventPublisher.CreateKweet
{
    public class KweetCreatedPublisher : IRequestHandler<KweetCreatedEvent>
    {
        private readonly IConnection _connection;

        public KweetCreatedPublisher(IConnection connection)
        {
            _connection = connection;
        }

        public async Task Handle(KweetCreatedEvent request, CancellationToken cancellationToken)
        {
            var @event = new KweetCreatedEvent
            {
                KweetId = request.KweetId,
                Text = request.Text,
                CustomerId = request.CustomerId,
                KweetCreatedDate = request.KweetCreatedDate
            };

            await PublishEvent(@event);
        }

        private async Task<bool> PublishEvent(KweetCreatedEvent @event)
        {
            var channel = _connection.CreateModel();
            var exchangeName = "kweet-created-exchange"; 
            var routingKey = "kweet.created";
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event));
            channel.BasicPublish(exchangeName, routingKey, null, body);

            return true;
        }
    }
}
