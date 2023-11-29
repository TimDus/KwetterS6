using MediatR;

namespace KweetService.API.Eventing.EventPublisher.KweetLiked
{
    public class KweetLikedEvent : IRequest
    {
        public int LikeId { get; set; }
        public int KweetId { get; set; }
        public int CustomerId { get; set; }
        public DateTime LikedDateTime { get; set; }
    }
}
