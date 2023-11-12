using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KweetService.API.Models.Entity
{
    [Table("Hashtag", Schema = "dbo")]
    public class HashtagEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [ForeignKey("kweet")]
        [Column("kweetid")]
        public int KweetId { get; set; }

        [Column("tag")]
        public string Tag { get; set; }

        public HashtagEntity(string tag, int kweetId)
        {
            Tag = tag;
            KweetId = kweetId;
        }
    }
}
