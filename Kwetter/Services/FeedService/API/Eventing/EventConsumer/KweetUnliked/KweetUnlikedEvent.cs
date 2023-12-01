using Common.Eventing;

namespace FeedService.API.Eventing.EventConsumer.KweetUnliked
{
    public class KweetUnlikedEvent : Event
    {
        public int LikeId { get; set; }
        public int KweetId { get; set; }
        public int CustomerId { get; set; }
        public DateTime LikedDateTime { get; set; }
    }
}
