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
        private readonly IConnection _connection;
        private readonly IServiceProvider _serviceProvider;

        public KweetCreatedConsumer(IServiceProvider serviceProvider)
        {
            _connection = serviceProvider.GetRequiredService<IConnection>();
            _model = _connection.CreateModel();
            _model.QueueDeclare(_queueName, durable: true, exclusive: false, autoDelete: false);
            _model.ExchangeDeclare("kweet-created-exchange", ExchangeType.Topic, durable: true, autoDelete: false);
            _model.QueueBind(_queueName, "kweet-created-exchange", string.Empty);
            _serviceProvider = serviceProvider;
        }

        const string _queueName = "kweet-created-queue";

        public async Task<KweetCreatedEvent> ReadMessages()
        {
            IFeedRepository _repository;
            using (var scope = _serviceProvider.CreateScope()) // this will use `IServiceScopeFactory` internally
            {
                _repository = scope.ServiceProvider.GetService<IFeedRepository>();
            }
            var consumer = new AsyncEventingBasicConsumer(_model);
            consumer.Received += async (ch, ea) =>
            {
                var body = ea.Body.ToArray();
                var text = System.Text.Encoding.UTF8.GetString(body);
                var json = Encoding.UTF8.GetString(body.ToArray());
                var kweetCreatedEvent = JsonSerializer.Deserialize<KweetCreatedEvent>(json);

                KweetEntity kweet = new KweetEntity()
                {
                    KweetCreatedId = kweetCreatedEvent.KweetId,
                    CreatedDate = kweetCreatedEvent.KweetCreatedDate,
                    CustomerId = kweetCreatedEvent.CustomerId,
                    Text = kweetCreatedEvent.Text
                };

                _repository.AddKweet(kweet);

                Console.WriteLine(text);
                await Task.CompletedTask;
                _model.BasicAck(ea.DeliveryTag, false);
            };
            _model.BasicConsume(_queueName, false, consumer);
            await Task.CompletedTask;
            return new KweetCreatedEvent();
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
