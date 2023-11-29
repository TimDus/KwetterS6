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

        public CustomerEntity Customer { get; set; } = new CustomerEntity();

        public string Text { get; set; }

        public DateTime CreatedDate { get; set; }

        public ICollection<HashtagEntity> Hashtags { get; set; } = new List<HashtagEntity>();

        public ICollection<MentionEntity>? Mentions { get; set; }

        public ICollection<KweetLikeEntity>? Likes { get; set; }

        public KweetEntity(string text, DateTime dateTime) 
        {
            Text = text;
            CreatedDate = dateTime;
        }

        public KweetEntity() { }
    }
}
