namespace KweetService.API.Models.DTO
{
    public class KweetCreatedDTO
    {
        public int? Id { get; set; }

        public int CustomerId { get; set; }

        public string Text { get; set; }

        public ICollection<HashtagDTO>? Hashtags { get; set; }

        public ICollection<MentionDTO>? Mentions { get; set; }
    }
}
