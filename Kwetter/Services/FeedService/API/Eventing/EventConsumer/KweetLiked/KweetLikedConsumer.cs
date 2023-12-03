using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using Common.Interfaces;
using System.Text;
using FeedService.API.Models.Entity;
using System.Text.Json;
using FeedService.API.Repositories.Interfaces;

namespace FeedService.API.Eventing.EventConsumer.KweetLiked
{
    public class KweetLikedConsumer : IConsumer<KweetLikedEvent>, IDisposable
    {
        private readonly IModel _model;
        private readonly IServiceProvider _serviceProvider;
        const string _queueName = "kweet-liked-queue";

        public KweetLikedConsumer(IServiceProvider serviceProvider)
        {
            _model = serviceProvider.GetRequiredService<IConnection>().CreateModel();
            _model.QueueDeclare(_queueName, durable: true, exclusive: false, autoDelete: false);
            _model.ExchangeDeclare("kweet-liked-exchange", ExchangeType.Topic, durable: true, autoDelete: false);
            _model.QueueBind(_queueName, "kweet-liked-exchange", string.Empty);
            _serviceProvider = serviceProvider;
        }

        public async Task<KweetLikedEvent> ReadMessages()
        {
            KweetLikedEvent kweetLikedEvent = new();
            var consumer = new AsyncEventingBasicConsumer(_model);
            consumer.Received += async (ch, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body.ToArray());
                kweetLikedEvent = JsonSerializer.Deserialize<KweetLikedEvent>(json);

                using (var scope = _serviceProvider.CreateScope()) // this will use `IServiceScopeFactory` internally
                {
                    var _kweetLikeRepository = scope.ServiceProvider.GetService<IKweetLikeRepository>();
                    var _kweetRepository = scope.ServiceProvider.GetService<IKweetRepository>();
                    var _customerRepository = scope.ServiceProvider.GetService<ICustomerRepository>();

                    KweetLikeEntity kweetLike = new(
                        kweetLikedEvent.LikeId,
                        await _customerRepository.GetById(kweetLikedEvent.CustomerId),
                        await _kweetRepository.GetById(kweetLikedEvent.CustomerId),
                        kweetLikedEvent.LikedDateTime
                        );

                    await _kweetLikeRepository.Create(kweetLike);
                }
                _model.BasicAck(ea.DeliveryTag, false);
            };
            _model.BasicConsume(_queueName, false, consumer);
            await Task.CompletedTask;
            return kweetLikedEvent;
        }

        public void Dispose()
        {
            if (_model.IsOpen)
                _model.Close();
        }
    }
}
