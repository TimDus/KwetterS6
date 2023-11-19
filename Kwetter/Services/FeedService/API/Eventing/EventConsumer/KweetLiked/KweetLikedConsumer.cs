using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using Common.Interfaces;
using FeedService.API.Repositories;

namespace FeedService.API.Eventing.EventConsumer.KweetLiked
{
    public class KweetLikedConsumer : IConsumer<KweetLikedEvent>, IDisposable
    {
        private readonly IModel _model;
        private readonly IConnection _connection;
        private readonly IServiceProvider _serviceProvider;

        public KweetLikedConsumer(IConsumerSetup setup, IServiceProvider serviceProvider)
        {
            _connection = setup.CreateChannel();
            _model = _connection.CreateModel();
            _model.QueueDeclare(_queueName, durable: true, exclusive: false, autoDelete: false);
            _model.ExchangeDeclare("kweet-liked-exchange", ExchangeType.Topic, durable: true, autoDelete: false);
            _model.QueueBind(_queueName, "kweet-liked-exchange", string.Empty);
            _serviceProvider = serviceProvider;
        }

        const string _queueName = "kweet-liked-queue";

        public async Task<KweetLikedEvent> ReadMessages()
        {
            using (var scope = _serviceProvider.CreateScope()) // this will use `IServiceScopeFactory` internally
            {
                var context = scope.ServiceProvider.GetService<IFeedRepository>();
            }
            var consumer = new AsyncEventingBasicConsumer(_model);
            consumer.Received += async (ch, ea) =>
            {
                var body = ea.Body.ToArray();
                var text = System.Text.Encoding.UTF8.GetString(body);
                Console.WriteLine(text);
                await Task.CompletedTask;
                _model.BasicAck(ea.DeliveryTag, false);
            };
            _model.BasicConsume(_queueName, false, consumer);
            await Task.CompletedTask;
            return new KweetLikedEvent();
        }

        public void Dispose()
        {
            if (_model.IsOpen)
                _model.Close();
            if (_connection.IsOpen)
                _connection.Close();
        }
    }
}
