using Common.Eventing;

namespace FollowService.API.Eventing.EventPublisher.CustomerUnfollowed
{
    public class CustomerUnfollowedEvent : Event
    {
        public long FollowerId { get; set; }
        public long FollowingId { get; set; }
    }
}
