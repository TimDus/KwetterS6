namespace KweetService.API.Models.DTO
{
    public class KweetLikeDTO
    {
        public int Id { get; set; }

        public int KweetId { get; set; }

        public int CustomerId { get; set; }

        public DateTime LikedDateTime { get; set; }
    }
}
