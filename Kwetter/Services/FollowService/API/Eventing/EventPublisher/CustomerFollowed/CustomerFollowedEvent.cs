using Common.Eventing;

namespace FollowService.API.Eventing.EventPublisher.CustomerFollowed
{
    public class CustomerFollowedEvent : Event
    {
        public int FollowId { get; set; }
        public int FollowerId { get; set; }
        public int FollowingId { get; set; }
        public DateTime FollowedDateTime { get; set; }
    }
}
