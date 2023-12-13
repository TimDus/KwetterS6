using MediatR;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace KweetService.API.Eventing.EventPublisher.KweetCreated
{
    public class KweetCreatedPublisher : IRequestHandler<KweetCreatedEvent>
    {
        private readonly IModel _model;

        public KweetCreatedPublisher(IConnection connection)
        {
            _model = connection.CreateModel();
        }

        public async Task Handle(KweetCreatedEvent request, CancellationToken cancellationToken)
        {
            await PublishEvent(request);
        }

        private async Task<bool> PublishEvent(KweetCreatedEvent @event)
        {
            var exchangeName = "kweet-created-exchange";
            var routingKey = "kweet.created";
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
