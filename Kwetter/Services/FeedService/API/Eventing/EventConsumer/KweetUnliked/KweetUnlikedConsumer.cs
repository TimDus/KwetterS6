using Common.Interfaces;
using FeedService.API.Models.Entity;
using FeedService.API.Repositories;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace FeedService.API.Eventing.EventConsumer.KweetUnliked
{
    public class KweetUnlikedConsumer : IConsumer<KweetUnlikedEvent>, IDisposable
    {
        private readonly IModel _model;
        private readonly IServiceProvider _serviceProvider;
        const string _queueName = "kweet-unliked-queue";

        public KweetUnlikedConsumer(IServiceProvider serviceProvider)
        {
            _model = serviceProvider.GetRequiredService<IConnection>().CreateModel();
            _model.QueueDeclare(_queueName, durable: true, exclusive: false, autoDelete: false);
            _model.ExchangeDeclare("kweet-unliked-exchange", ExchangeType.Topic, durable: true, autoDelete: false);
            _model.QueueBind(_queueName, "kweet-unliked-exchange", string.Empty);
            _serviceProvider = serviceProvider;
        }

        public async Task<KweetUnlikedEvent> ReadMessages()
        {
            KweetUnlikedEvent kweetUnlikedEvent = new();
            var consumer = new AsyncEventingBasicConsumer(_model);
            consumer.Received += async (ch, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body.ToArray());
                kweetUnlikedEvent = JsonSerializer.Deserialize<KweetUnlikedEvent>(json);

                using (var scope = _serviceProvider.CreateScope()) // this will use `IServiceScopeFactory` internally
                {
                    var _repository = scope.ServiceProvider.GetService<IFeedRepository>();

                    await _repository.UnlikeKweet(kweetUnlikedEvent.LikeId);
                }
                _model.BasicAck(ea.DeliveryTag, false);
            };
            _model.BasicConsume(_queueName, false, consumer);
            await Task.CompletedTask;
            return kweetUnlikedEvent;
        }

        public void Dispose()
        {
            if (_model.IsOpen)
                _model.Close();
        }
    }
}
