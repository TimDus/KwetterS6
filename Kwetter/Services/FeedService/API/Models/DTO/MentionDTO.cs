namespace FeedService.API.Models.DTO
{
    public class MentionDTO
    {
        public int Id { get; set; }

        public int MentionedCustomerId { get; set; }

        public int KweetId { get; set; }

        public MentionDTO() { } 

        public MentionDTO(int kweetServiceId, int mentionedCustomerId, int kweetId)
        {
            Id = kweetServiceId;
            MentionedCustomerId = mentionedCustomerId;
            KweetId = kweetId;
        }
    }
}
