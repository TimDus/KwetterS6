using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KweetService.API.Models.Entity
{
    [Table("Hashtags", Schema = "dbo")]
    public class HashtagEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public KweetEntity? Kweet { get; set; }

        public string Tag { get; set; }

        public HashtagEntity(string tag)
        {
            Tag = tag;
        }

        public HashtagEntity() { }
    }
}
