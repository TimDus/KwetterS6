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

        public async Task Handle(KweetUnlikedEvent request, CancellationToken cancellationToken)
        {
            var @event = new KweetUnlikedEvent
            {
            };

            await PublishEvent(@event);
        }

        private async Task<bool> PublishEvent(KweetUnlikedEvent @event)
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
