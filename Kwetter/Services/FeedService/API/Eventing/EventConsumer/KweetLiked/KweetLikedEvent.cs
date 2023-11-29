using Common.Eventing;

namespace FeedService.API.Eventing.EventConsumer.KweetLiked
{
    public class KweetLikedEvent : Event
    {
        public int KweetId { get; set; }
        public int CustomerId { get; set; }
        public string Text { get; set; }
        public DateTime KweetCreatedDate { get; set; }
    }
}
