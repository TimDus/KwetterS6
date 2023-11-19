using Common.Eventing;

namespace FeedService.API.Eventing.EventConsumer.KweetLiked
{
    public class KweetLikedEvent : Event
    {
        public long KweetId { get; set; }
        public long CustomerId { get; set; }
        public string Text { get; set; }
        public DateTime KweetCreatedDate { get; set; }
    }
}
