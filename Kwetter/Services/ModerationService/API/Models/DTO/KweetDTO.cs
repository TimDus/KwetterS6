namespace ModerationService.API.Models.DTO
{
    public class KweetDTO
    {
        public int KweetServiceId { get; set; }

        public int CustomerId { get; set; }

        public string CustomerName { get; set; }

        public string Text { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
