namespace FollowService.API.Models.DTO
{
    public class FollowDTO
    {
        public int FollowerId { get; set; }

        public int FollowingId { get; set; }

        public DateTime FollowedDateTime { get; set; }
    }
}
