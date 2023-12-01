using Common.Interfaces;

namespace FeedService.API.Eventing.EventConsumer.KweetUnliked
{
    public class KweetUnlikedHosted : BackgroundService
    {
        private readonly IConsumer<KweetUnlikedEvent> _consumer;

        public KweetUnlikedHosted(IConsumer<KweetUnlikedEvent> consumer)
        {
            _consumer = consumer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _consumer.ReadMessages();
        }
    }
}
