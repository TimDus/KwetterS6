using Common.Eventing;

namespace FeedService.API.Eventing.EventConsumer.CustomerUnfollowed
{
    public class CustomerUnfollowedEvent : Event
    {
        public long FollowerId { get; set; }
        public long FollowingId { get; set; }
    }
}
