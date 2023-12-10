using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FeedService.API.Models.Entity
{
    [Table("Hashtags", Schema = "dbo")]
    public class HashtagEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int KweetServiceId { get; set; }

        public KweetEntity Kweet { get; set; }

        public string Tag { get; set; }

        public HashtagEntity(int kweetServiceId, string tag)
        {
            KweetServiceId = kweetServiceId;
            Tag = tag;
        }

        public HashtagEntity() { }
    }
}
