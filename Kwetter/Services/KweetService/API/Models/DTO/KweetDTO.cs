using KweetService.API.Models.Entity;

namespace KweetService.API.Models.DTO
{
    public class KweetDTO
    {
        public int Id { get; set; }

        public CustomerDTO Customer { get; set; }

        public string Text { get; set; }

        public DateTime CreatedDate { get; set; }

        public ICollection<HashtagDTO>? Hashtags { get; set; }

        public ICollection<MentionDTO>? Mentions { get; set; }

        public ICollection<KweetLikeDTO>? Likes { get; set; }
    }
}
