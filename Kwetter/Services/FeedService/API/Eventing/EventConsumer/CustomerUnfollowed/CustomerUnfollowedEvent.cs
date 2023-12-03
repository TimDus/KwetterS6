using Common.Eventing;

namespace FeedService.API.Eventing.EventConsumer.CustomerUnfollowed
{
    public class CustomerUnfollowedEvent : Event
    {
        public int FollowServiceId { get; set; }
        public int FollowerId { get; set; }
        public int FollowingId { get; set; }
    }
}
