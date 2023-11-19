using KweetService.API.Models.Entity;

namespace KweetService.API.Models.DTO
{
    public class KweetLikeDTO
    {
        public int Id { get; set; }

        public KweetDTO Kweet { get; set; }

        public CustomerDTO Customer { get; set; }

        public DateTime LikedDateTime { get; set; }
    }
}
