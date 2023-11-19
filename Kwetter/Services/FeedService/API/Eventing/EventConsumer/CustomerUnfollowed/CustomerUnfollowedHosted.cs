using Common.Interfaces;

namespace FeedService.API.Eventing.EventConsumer.CustomerUnfollowed
{
    public class CustomerUnfollowedHosted : BackgroundService
    {
        private readonly IConsumer<CustomerUnfollowedEvent> _consumer;

        public CustomerUnfollowedHosted(IConsumer<CustomerUnfollowedEvent> consumer)
        {
            _consumer = consumer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _consumer.ReadMessages();
        }
    }
}
