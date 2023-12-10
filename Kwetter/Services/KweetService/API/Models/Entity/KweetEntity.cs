using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KweetService.API.Models.Entity
{
    [Table("Kweets", Schema = "dbo")]
    public class KweetEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public CustomerEntity Customer { get; set; } = new CustomerEntity();

        public string Text { get; set; }

        public DateTime CreatedDate { get; set; }

        public ICollection<HashtagEntity> Hashtags { get; set; } = new List<HashtagEntity>();

        public ICollection<MentionEntity>? Mentions { get; set; } = new List<MentionEntity>();

        public ICollection<KweetLikeEntity>? Likes { get; set; }

        public KweetEntity(string text, DateTime dateTime)
        {
            Text = text;
            CreatedDate = dateTime;
        }

        public KweetEntity() { }
    }
}
