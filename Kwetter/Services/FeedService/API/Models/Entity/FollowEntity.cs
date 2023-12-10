using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FeedService.API.Models.Entity
{
    [Table("Follows", Schema = "dbo")]
    public class FollowEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int FollowServiceId { get; set; }

        public CustomerEntity Follower { get; set; }

        public CustomerEntity Following { get; set; }

        public DateTime FollowedDateTime { get; set; }

        public FollowEntity() { }

        public FollowEntity(int followServiceId, CustomerEntity follower, CustomerEntity following, DateTime followedDateTime)
        {
            FollowServiceId = followServiceId;
            Follower = follower;
            Following = following;
            FollowedDateTime = followedDateTime;
        }
    }
}
