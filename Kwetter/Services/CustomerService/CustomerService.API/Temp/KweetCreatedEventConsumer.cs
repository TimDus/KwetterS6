using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Configuration;
using Common;

namespace CustomerService.API.Temp
{
    public class KweetCreatedEventConsumer : AsyncEventingBasicConsumer
    {
        public KweetCreatedEventConsumer(IModel model) : base(model)
        {}

        public override async Task HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, ReadOnlyMemory<byte> body)
        {
            var json = Encoding.UTF8.GetString(body.ToArray());
            var kweetCreateEvent = JsonSerializer.Deserialize<KweetCreateEvent>(json);

            Console.WriteLine(kweetCreateEvent.Kweet);

            Model.BasicAck(deliveryTag, false);
        }
    }
}
