using Common.Interfaces;
using FeedService.API.Repositories.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace FeedService.API.Eventing.EventConsumer.CustomerUnfollowed
{
    public class CustomerUnfollowedConsumer : IConsumer<CustomerUnfollowedEvent>, IDisposable
    {
        private readonly IModel _model;
        private readonly IServiceProvider _serviceProvider;
        const string _queueName = "customer-unfollowed-queue";

        public CustomerUnfollowedConsumer(IServiceProvider serviceProvider)
        {
            _model = serviceProvider.GetRequiredService<IConnection>().CreateModel();
            _model.QueueDeclare(_queueName, durable: true, exclusive: false, autoDelete: false);
            _model.ExchangeDeclare("customer-unfollowed-exchange", ExchangeType.Topic, durable: true, autoDelete: false);
            _model.QueueBind(_queueName, "customer-unfollowed-exchange", string.Empty);
            _serviceProvider = serviceProvider;
        }

        public async Task<CustomerUnfollowedEvent> ReadMessages()
        {
            CustomerUnfollowedEvent unfollowedEvent = new();
            var consumer = new AsyncEventingBasicConsumer(_model);
            consumer.Received += async (ch, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body.ToArray());
                unfollowedEvent = JsonSerializer.Deserialize<CustomerUnfollowedEvent>(json);

                using (var scope = _serviceProvider.CreateScope()) // this will use `IServiceScopeFactory` internally
                {
                    var _repository = scope.ServiceProvider.GetService<IFollowRepository>();

                    await _repository.Delete(unfollowedEvent.FollowServiceId);
                }

                _model.BasicAck(ea.DeliveryTag, false);
            };
            _model.BasicConsume(_queueName, false, consumer);
            await Task.CompletedTask;
            return unfollowedEvent;
        }

        public void Dispose()
        {
            if (_model.IsOpen)
                _model.Close();
        }
    }
}
