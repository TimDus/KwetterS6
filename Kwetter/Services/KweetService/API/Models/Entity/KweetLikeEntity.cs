using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KweetService.API.Models.Entity
{
    [Table("KweetLikes", Schema = "dbo")]
    public class KweetLikeEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public CustomerEntity Customer { get; set; }

        public KweetEntity Kweet { get; set; }

        public DateTime LikedDateTime { get; set; }

        public KweetLikeEntity() { }

        public KweetLikeEntity(DateTime likedDatedTime, CustomerEntity customer, KweetEntity kweet)
        {
            LikedDateTime = likedDatedTime;
            Customer = customer;
            Kweet = kweet;
        }
    }
}
