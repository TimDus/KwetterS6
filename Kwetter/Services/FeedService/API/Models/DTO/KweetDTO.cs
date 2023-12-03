namespace FeedService.API.Models.DTO
{
    public class KweetDTO
    {
        public int KweetServiceId { get; set; }

        public int CustomerId { get; set; }

        public string CustomerName { get; set; }

        public string DisplayName { get; set; }

        public string ProfilePicture { get; set; }

        public string Text { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool Liked { get; set; }

        public ICollection<HashtagDTO> Hashtags { get; set; } = new List<HashtagDTO>();

        public ICollection<MentionDTO>? Mentions { get; set; } = new List<MentionDTO>();
    }
}
