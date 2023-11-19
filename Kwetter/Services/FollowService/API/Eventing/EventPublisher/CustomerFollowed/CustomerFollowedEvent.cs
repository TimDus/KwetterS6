using Common.Eventing;

namespace FollowService.API.Eventing.EventPublisher.CustomerFollowed
{
    public class CustomerFollowedEvent : Event
    {
        public long FollowerId { get; set; }
        public long FollowingId { get; set; }
        public DateTime FollowedDateTime { get; set; }
    }
}
