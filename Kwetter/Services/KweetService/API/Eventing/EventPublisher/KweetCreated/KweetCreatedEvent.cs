using Common.Eventing;
using KweetService.API.Models.DTO;

namespace KweetService.API.Eventing.EventPublisher.KweetCreated
{
    public class KweetCreatedEvent : Event
    {
        public int KweetId { get; set; }
        public int CustomerId { get; set; }
        public string Text { get; set; }
        public ICollection<HashtagDTO>? Hashtags { get; set; }
        public DateTime KweetCreatedDate { get; set; }
    }
}
