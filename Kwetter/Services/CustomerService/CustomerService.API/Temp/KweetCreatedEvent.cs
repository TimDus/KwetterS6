namespace CustomerService.API.Temp
{
    public class KweetCreatedEvent
    {
        public Guid Id { get; set; }
        public string kweet { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
