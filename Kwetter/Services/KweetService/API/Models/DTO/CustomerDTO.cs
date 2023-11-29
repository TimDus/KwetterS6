namespace KweetService.API.Models.DTO
{
    public class CustomerDTO
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public string DisplayName { get; set; }

        public string CustomerName { get; set; }

        public string? ProfilePicture { get; set; }

        public List<KweetCreateDTO>? Kweets { get; set; }

        public List<KweetLikeDTO>? LikedKweets { get; set; }

        public List<MentionDTO>? MentionedBy { get; set; }
    }
}
