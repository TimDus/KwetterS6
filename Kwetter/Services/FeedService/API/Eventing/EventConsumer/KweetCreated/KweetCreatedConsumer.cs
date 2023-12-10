using Common.Interfaces;
using FeedService.API.Models.DTO;
using FeedService.API.Models.Entity;
using FeedService.API.Repositories.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace FeedService.API.Eventing.EventConsumer.KweetCreated
{
    public class KweetCreatedConsumer : IConsumer<KweetCreatedEvent>, IDisposable
    {
        private readonly IModel _model;
        private readonly IServiceProvider _serviceProvider;
        const string _queueName = "feed-kweet-created-queue";

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

                foreach (HashtagDTO hashtag in kweetCreatedEvent.Hashtags)
                {
                    kweet.Hashtags.Add(new HashtagEntity(hashtag.Id, hashtag.Tag));
                }

                using (var scope = _serviceProvider.CreateScope()) // this will use `IServiceScopeFactory` internally
                {
                    var _kweetRepository = scope.ServiceProvider.GetService<IKweetRepository>();
                    var _customerRepository = scope.ServiceProvider.GetService<ICustomerRepository>();

                    foreach (MentionDTO mention in kweetCreatedEvent.Mentions)
                    {
                        kweet.Mentions.Add(new MentionEntity(mention.Id, await _customerRepository.GetById(mention.MentionedCustomerId)));
                    }

                    kweet.Customer = await _customerRepository.GetById(kweetCreatedEvent.CustomerId);
                    await _kweetRepository.Create(kweet);
                }

                _model.BasicAck(ea.DeliveryTag, false);
            };
            _model.BasicConsume(_queueName, false, consumer);
            await Task.CompletedTask;
            return kweetCreatedEvent;
        }

        public void Dispose()
        {
            if (_model.IsOpen)
                _model.Close();
        }
    }
}
