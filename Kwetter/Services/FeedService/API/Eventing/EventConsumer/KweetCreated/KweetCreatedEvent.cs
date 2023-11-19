using Common.Eventing;

namespace FeedService.API.Eventing.EventConsumer.KweetCreated
{
    public class KweetCreatedEvent : Event
    {
        public long KweetId { get; set; }
        public long CustomerId { get; set; }
        public string Text { get; set; }
        public DateTime KweetCreatedDate { get; set; }
    }
}
