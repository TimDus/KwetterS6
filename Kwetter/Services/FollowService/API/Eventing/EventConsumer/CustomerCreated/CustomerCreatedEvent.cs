namespace FollowService.API.Eventing.EventConsumer.CustomerCreated
{
    public class CustomerCreatedEvent
    {
        public int CustomerId { get; set; }

        public string DisplayName { get; set; }

        public string CustomerName { get; set; }

        public string CustomerProfilePicture { get; set; }
    }
}
