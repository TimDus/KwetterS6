using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using FeedService.API.Repositories;
using FeedService.API.Models.Entity;
using Common.Eventing;

namespace FeedService.API.Eventing.EventReceiver.KweetCreated
{
    public class KweetCreatedConsumer : AsyncEventingBasicConsumer
    {
        private readonly IFeedRepository _repository;

        public KweetCreatedConsumer(IModel model) : base(model)
        {
        }

        public override async Task HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, ReadOnlyMemory<byte> body)
        {
            var json = Encoding.UTF8.GetString(body.ToArray());
            var kweetCreatedEvent = JsonSerializer.Deserialize<KweetCreatedEvent>(json);

            Console.WriteLine(kweetCreatedEvent.KweetId);

            KweetEntity kweet = new KweetEntity();
            _repository.AddKweet(kweet);

            Model.BasicAck(deliveryTag, false);
        }
    }
}
