using MediatR;

namespace KweetService.API.Eventing.EventPublisher.KweetUnliked
{
    public class KweetUnlikedEvent : IRequest
    {
        public long LikeId { get; set; }
        public long KweetId { get; set; }
        public long CustomerId { get; set; }
        public DateTime LikedDateTime { get; set; }
    }
}
