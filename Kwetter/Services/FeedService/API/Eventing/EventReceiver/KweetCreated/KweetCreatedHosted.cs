using Common.Interfaces;
using FeedService.API.Repositories;

namespace FeedService.API.Eventing.EventReceiver.KweetCreated
{
    public class KweetCreatedHosted : BackgroundService
    {
        private readonly IConsumer<KweetCreatedEvent> _consumer;

        public KweetCreatedHosted(IConsumer<KweetCreatedEvent> consumer)
        {
            _consumer = consumer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _consumer.ReadMessages();
        }
    }
}
