using Common.Eventing;

namespace FeedService.API.Eventing.EventConsumer.CustomerFollowed
{
    public class CustomerFollowedEvent : Event
    {
        public int FollowId { get; set; }
        public int FollowerId { get; set; }
        public int FollowingId { get; set; }
        public DateTime FollowedDateTime { get; set; }
    }
}
