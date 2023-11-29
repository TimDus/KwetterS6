using Common.Eventing;

namespace FeedService.API.Eventing.EventConsumer.KweetCreated
{
    public class KweetCreatedEvent : Event
    {
        public int KweetId { get; set; }
        public int CustomerId { get; set; }
        public string Text { get; set; }
        public DateTime KweetCreatedDate { get; set; }
    }
}
