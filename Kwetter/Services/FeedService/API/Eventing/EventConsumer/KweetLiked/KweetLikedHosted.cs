using Common.Interfaces;

namespace FeedService.API.Eventing.EventConsumer.KweetLiked
{
    public class KweetLikedHosted : BackgroundService
    {
        private readonly IConsumer<KweetLikedEvent> _consumer;

        public KweetLikedHosted(IConsumer<KweetLikedEvent> consumer)
        {
            _consumer = consumer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _consumer.ReadMessages();
        }
    }
}
