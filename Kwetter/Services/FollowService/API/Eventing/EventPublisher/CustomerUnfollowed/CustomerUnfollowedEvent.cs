using Common.Eventing;

namespace FollowService.API.Eventing.EventPublisher.CustomerUnfollowed
{
    public class CustomerUnfollowedEvent : Event
    {
        public int FollowId { get; set; }
        public int FollowerId { get; set; }
        public int FollowingId { get; set; }
    }
}
