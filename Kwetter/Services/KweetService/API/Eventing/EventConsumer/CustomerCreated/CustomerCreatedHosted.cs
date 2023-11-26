using Common.Interfaces;
using KweetService.API.Eventing.EventReceiver.CustomerCreated;

namespace KweetService.API.Eventing.EventConsumer.CustomerCreated
{
    public class CustomerCreatedHosted : BackgroundService
    {
        private readonly IConsumer<CustomerCreatedEvent> _consumer;

        public CustomerCreatedHosted(IConsumer<CustomerCreatedEvent> consumer)
        {
            _consumer = consumer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _consumer.ReadMessages();
        }
    }
}
