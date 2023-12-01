using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FollowService.API.Models.Entity
{
    public class FollowEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public CustomerEntity Follower { get; set; }

        public CustomerEntity Following { get; set; }

        public DateTime FollowedDateTime { get; set; }

        public FollowEntity() { }

        public FollowEntity(CustomerEntity follower, CustomerEntity following, DateTime followedDateTime)
        {
            Follower = follower;
            Following = following;
            FollowedDateTime = followedDateTime;
        }
    }
}
