namespace CustomerService.API.Temp
{
    public class KweetCreateEvent
    {
        public Guid Id { get; set; }
        public string kweet { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
