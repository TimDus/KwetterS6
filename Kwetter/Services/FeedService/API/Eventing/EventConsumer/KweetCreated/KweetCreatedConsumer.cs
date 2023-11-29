using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using Common.Interfaces;
using FeedService.API.Repositories;
using System.Text;
using System.Text.Json;
using FeedService.API.Models.Entity;

namespace FeedService.API.Eventing.EventConsumer.KweetCreated
{
    public class KweetCreatedConsumer : IConsumer<KweetCreatedEvent>, IDisposable
    {
        private readonly IModel _model;
        private readonly IServiceProvider _serviceProvider;
        const string _queueName = "kweet-created-queue";

        public KweetCreatedConsumer(IServiceProvider serviceProvider)
        {
            _model = serviceProvider.GetRequiredService<IConnection>().CreateModel();
            _model.QueueDeclare(_queueName, durable: true, exclusive: false, autoDelete: false);
            _model.ExchangeDeclare("kweet-created-exchange", ExchangeType.Topic, durable: true, autoDelete: false);
            _model.QueueBind(_queueName, "kweet-created-exchange", string.Empty);
            _serviceProvider = serviceProvider;
        }

        public async Task<KweetCreatedEvent> ReadMessages()
        {
            KweetCreatedEvent kweetCreatedEvent = new();
            var consumer = new AsyncEventingBasicConsumer(_model);
            consumer.Received += async (ch, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body.ToArray());
                kweetCreatedEvent = JsonSerializer.Deserialize<KweetCreatedEvent>(json);

                KweetEntity kweet = new(kweetCreatedEvent.KweetId, kweetCreatedEvent.Text, kweetCreatedEvent.KweetCreatedDate);

                using (var scope = _serviceProvider.CreateScope()) // this will use `IServiceScopeFactory` internally
                {
                    var _repository = scope.ServiceProvider.GetService<IFeedRepository>();

                    kweet.Customer = await _repository.GetCustomer(kweetCreatedEvent.CustomerId);
                    await _repository.CreateKweet(kweet);
                }

                _model.BasicAck(ea.DeliveryTag, false);
            };
            _model.BasicConsume(_queueName, false, consumer);
            return kweetCreatedEvent;
        }

        public void Dispose()
        {
            if (_model.IsOpen)
                _model.Close();
        }
    }
}
