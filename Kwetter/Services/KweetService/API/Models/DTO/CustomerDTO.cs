using KweetService.API.Models.Entity;

namespace KweetService.API.Models.DTO
{
    public class CustomerDTO
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public string DisplayName { get; set; }

        public string CustomerName { get; set; }

        public string? ProfilePicture { get; set; }

        public ICollection<KweetDTO>? Kweets { get; set; }

        public ICollection<KweetLikeDTO>? LikedKweets { get; set; }

        public ICollection<MentionDTO>? MentionedBy { get; set; }
    }
}
