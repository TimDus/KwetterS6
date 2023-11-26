using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FeedService.API.Models.Entity
{
    public class KweetEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Key]
        [Column("id")]
        public long KweetCreatedId { get; set; }

        [ForeignKey("customer")]
        [Column("customerid")]
        public long CustomerId { get; set; }

        [Column("text")]
        public string Text { get; set; }

        [Column("createddate")]
        public DateTime CreatedDate { get; set; }

        //public ICollection<HashtagEntity>? Hashtags { get; set; }

        //public ICollection<MentionEntity>? Mentions { get; set; }

        //public ICollection<KweetLikeEntity>? Likes { get; set; }
    }
}
