using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KweetService.API.Models.Entity
{
    [Table("Kweet", Schema = "dbo")]
    public class KweetEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("CustomerEntityId")]
        public CustomerEntity Customer { get; set; }

        public string Text { get; set; }

        public DateTime CreatedDate { get; set; }

        public ICollection<HashtagEntity>? Hashtags { get; set; }

        public ICollection<MentionEntity>? Mentions { get; set; }

        public ICollection<KweetLikeEntity>? Likes { get; set; }

        public KweetEntity(CustomerEntity customer, string text, DateTime dateTime) 
        {
            this.Customer = customer;
            this.Text = text;
            this.CreatedDate = dateTime;
        }

        public KweetEntity(){}
    }
}
