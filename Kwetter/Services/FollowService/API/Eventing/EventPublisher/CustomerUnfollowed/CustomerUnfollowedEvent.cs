using Common.Eventing;

namespace FollowService.API.Eventing.EventPublisher.CustomerUnfollowed
{
    public class CustomerUnfollowedEvent : Event
    {
        public int FollowServiceId { get; set; }
        public int FollowerId { get; set; }
        public int FollowingId { get; set; }
    }
}
