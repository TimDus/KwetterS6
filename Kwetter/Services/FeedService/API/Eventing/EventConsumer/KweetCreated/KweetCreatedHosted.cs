using Common.Interfaces;
using KweetService.API.Eventing.EventPublisher.KweetCreated;

namespace FeedService.API.Eventing.EventConsumer.KweetCreated
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
