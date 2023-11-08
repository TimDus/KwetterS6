namespace KweetService.API.Models.DTO
{
    public class KweetDTO
    {
        public long Id { get; set; }
        public long CustomerId { get; set; }
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
