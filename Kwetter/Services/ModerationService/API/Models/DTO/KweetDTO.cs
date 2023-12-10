namespace ModerationService.API.Models.DTO
{
    public class KweetDTO
    {
        public int KweetServiceId { get; set; }

        public int CustomerId { get; set; }

        public string CustomerName { get; set; }

        public string Text { get; set; }

        public DateTime CreatedDate { get; set; }

        public KweetDTO() { }

        public KweetDTO(int kweetServiceId, int customerId, string customerName, string text, DateTime createdDate)
        {
            KweetServiceId = kweetServiceId;
            CustomerId = customerId;
            CustomerName = customerName;
            Text = text;
            CreatedDate = createdDate;
        }
    }
}
