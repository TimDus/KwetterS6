using KweetService.API.Models.Entity;

namespace KweetService.API.Models.DTO
{
    public class CustomerDTO
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }

        public ICollection<KweetDTO>? Kweets { get; set; }

        public ICollection<KweetLikeDTO>? Likes { get; set; }

        public ICollection<MentionDTO>? Mentions { get; set; }
    }
}
