using MediatR;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace KweetService.API.Eventing.EventPublisher.KweetUnliked
{
    public class KweetUnlikedPublisher : IRequestHandler<KweetUnlikedEvent>
    {
        private readonly IModel _model;

        public KweetUnlikedPublisher(IConnection connection)
        {
            _model = connection.CreateModel();
        }

        public async Task Handle(KweetUnlikedEvent @event, CancellationToken cancellationToken)
        {
            await PublishEvent(@event);
        }

        private async Task<bool> PublishEvent(KweetUnlikedEvent @event)
        {
            var exchangeName = "kweet-unliked-exchange";
            var routingKey = "kweet.unliked";
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
