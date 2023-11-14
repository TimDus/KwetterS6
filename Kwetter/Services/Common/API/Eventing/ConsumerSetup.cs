using Common.Interfaces;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Common.Eventing
{
    public class ConsumerSetup : IConsumerSetup
    {
        private readonly RabbitMqConfiguration _configuration;
        public ConsumerSetup(IOptions<RabbitMqConfiguration> options)
        {
            _configuration = options.Value;
        }
        public IConnection CreateChannel()
        {
            ConnectionFactory connection = new ConnectionFactory()
            {
                UserName = _configuration.UserName,
                Password = _configuration.Password,
                HostName = _configuration.HostName
            };
            connection.DispatchConsumersAsync = true;
            var channel = connection.CreateConnection();
            return channel;
        }
    }
}
