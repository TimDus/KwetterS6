using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FeedService.API.Models.Entity
{
    [Table("KweetLikes", Schema = "dbo")]
    public class KweetLikeEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int KweetLikeServiceId { get; set; }

        public CustomerEntity Customer { get; set; }

        public KweetEntity Kweet { get; set; }

        public DateTime LikedDateTime { get; set; }

        public KweetLikeEntity() { }

        public KweetLikeEntity(int kweetLikeServiceId, CustomerEntity customer, KweetEntity kweet, DateTime likedDateTime)
        {
            KweetLikeServiceId = kweetLikeServiceId;
            Customer = customer;
            Kweet = kweet;
            LikedDateTime = likedDateTime;
        }
    }
}
