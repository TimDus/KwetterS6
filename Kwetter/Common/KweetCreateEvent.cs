namespace Common
{
    public class KweetCreateEvent
    {
        public Guid Id { get; set; }
        public string Kweet { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}