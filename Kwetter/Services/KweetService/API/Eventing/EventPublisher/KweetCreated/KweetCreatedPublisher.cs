using MediatR;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace KweetService.API.Eventing.EventPublisher.KweetCreated
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
            await PublishEvent(request);
        }

        private async Task<bool> PublishEvent(KweetCreatedEvent @event)
        {
            var channel = _connection.CreateModel();
            var exchangeName = "kweet-created-exchange";
            var routingKey = "kweet.created";
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event));
            channel.BasicPublish(exchangeName, routingKey, null, body);
            await Task.CompletedTask;
            return true;
        }
    }
}
