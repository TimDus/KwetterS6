namespace KweetService.API.Models.DTO
{
    public class MentionDTO
    {
        public int Id { get; set; }

        public int MentionedCustomerId { get; set; }

        public int KweetId { get; set; }
    }
}
