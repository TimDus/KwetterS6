using Common.Eventing;

namespace FollowService.API.Eventing.EventPublisher.CustomerFollowed
{
    public class CustomerFollowedEvent : Event
    {
        public int FollowServiceId { get; set; }
        public int FollowerId { get; set; }
        public int FollowingId { get; set; }
        public DateTime FollowedDateTime { get; set; }
    }
}
