using Common.Interfaces;
using FeedService.API.Models.Entity;
using FeedService.API.Repositories.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace FeedService.API.Eventing.EventConsumer.CustomerFollowed
{
    public class CustomerFollowedConsumer : IConsumer<CustomerFollowedEvent>, IDisposable
    {
        private readonly IModel _model;
        private readonly IConnection _connection;
        private readonly IServiceProvider _serviceProvider;

        public CustomerFollowedConsumer(IServiceProvider serviceProvider)
        {
            _connection = serviceProvider.GetRequiredService<IConnection>();
            _model = _connection.CreateModel();
            _model.QueueDeclare(_queueName, durable: true, exclusive: false, autoDelete: false);
            _model.ExchangeDeclare("customer-followed-exchange", ExchangeType.Topic, durable: true, autoDelete: false);
            _model.QueueBind(_queueName, "customer-followed-exchange", string.Empty);
            _serviceProvider = serviceProvider;
        }

        const string _queueName = "customer-followed-queue";

        public async Task<CustomerFollowedEvent> ReadMessages()
        {
            CustomerFollowedEvent followedEvent = new();
            var consumer = new AsyncEventingBasicConsumer(_model);
            consumer.Received += async (ch, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body.ToArray());
                followedEvent = JsonSerializer.Deserialize<CustomerFollowedEvent>(json);

                using (var scope = _serviceProvider.CreateScope()) // this will use `IServiceScopeFactory` internally
                {
                    var _followRepository = scope.ServiceProvider.GetService<IFollowRepository>();
                    var _customerRepository = scope.ServiceProvider.GetService<ICustomerRepository>();

                    FollowEntity followEntity = new(
                        followedEvent.FollowServiceId,
                        await _customerRepository.GetById(followedEvent.FollowerId),
                        await _customerRepository.GetById(followedEvent.FollowingId),
                        followedEvent.FollowedDateTime
                        );

                    await _followRepository.Create(followEntity);
                }
                _model.BasicAck(ea.DeliveryTag, false);
            };
            _model.BasicConsume(_queueName, false, consumer);
            await Task.CompletedTask;
            return followedEvent;
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
