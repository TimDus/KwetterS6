using Common.Eventing;

namespace FeedService.API.Eventing.EventConsumer.CustomerFollowed
{
    public class CustomerFollowedEvent : Event
    {
        public long FollowerId { get; set; }
        public long FollowingId { get; set; }
        public DateTime FollowedDateTime { get; set; }
    }
}
