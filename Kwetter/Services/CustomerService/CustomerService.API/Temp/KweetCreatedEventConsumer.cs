using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Configuration;

namespace CustomerService.API.Temp
{
    public class KweetCreatedEventConsumer : AsyncEventingBasicConsumer
    {
        public KweetCreatedEventConsumer(IModel model) : base(model){}

        public async Task HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, ReadOnlyMemory<byte> body)
        {
            var json = Encoding.UTF8.GetString(body.ToArray());
            var postCreateEvent = JsonSerializer.Deserialize<KweetCreatedEvent>(json);

            Console.WriteLine(postCreateEvent.kweet);

            Model.BasicAck(deliveryTag, false);
        }
    }
}
