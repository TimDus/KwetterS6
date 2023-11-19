using Common.Interfaces;

namespace FeedService.API.Eventing.EventConsumer.CustomerFollowed
{
    public class CustomerFollowedHosted : BackgroundService
    {
        private readonly IConsumer<CustomerFollowedEvent> _consumer;

        public CustomerFollowedHosted(IConsumer<CustomerFollowedEvent> consumer)
        {
            _consumer = consumer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _consumer.ReadMessages();
        }
    }
}
