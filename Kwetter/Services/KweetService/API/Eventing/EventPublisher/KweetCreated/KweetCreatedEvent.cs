using Common.Eventing;

namespace KweetService.API.Eventing.EventPublisher.KweetCreated
{
    public class KweetCreatedEvent : Event
    {
        public long KweetId { get; set; }
        public long CustomerId { get; set; }
        public string Text { get; set; }
        public DateTime KweetCreatedDate { get; set; }
    }
}
