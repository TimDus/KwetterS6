using Common.Eventing;

namespace CustomerService.API.Eventing.EventPublisher.CustomerCreated
{
    public class CustomerCreatedEvent : Event
    {
        public int CustomerId { get; set; }

        public string DisplayName { get; set; }

        public string CustomerName { get; set; }
    }
}
