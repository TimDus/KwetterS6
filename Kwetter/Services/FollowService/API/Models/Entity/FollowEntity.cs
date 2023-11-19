using System.ComponentModel.DataAnnotations;

namespace FollowService.API.Models.Entity
{
    public class FollowEntity
    {
        [Key]
        public int FollowerId { get; set; }

        [Key]
        public int FollowingId { get; set; }

        public DateTime FollowedDateTime { get; set; }
    }
}
