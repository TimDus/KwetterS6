using Common.Eventing;

namespace ModerationService.API.Eventing.EventConsumer.CustomerCreated
{
    public class CustomerCreatedEvent : Event
    {
        public int CustomerId { get; set; }

        public string DisplayName { get; set; }

        public string CustomerName { get; set; }
    }
}
