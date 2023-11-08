using Common;

namespace CustomerService.API.Temp
{
    internal class KweetCreatedEvent : Event
    {
        public Guid KweetId { get; set; }
        public string Kweet { get; set; }
        public string Author { get; set; }
        public DateTime KweetCreatedDate { get; set; }
    }
}