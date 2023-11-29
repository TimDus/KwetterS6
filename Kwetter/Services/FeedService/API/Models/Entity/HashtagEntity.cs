using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FeedService.API.Models.Entity
{
    public class HashtagEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public KweetEntity Kweet { get; set; }

        public string Tag { get; set; }

        public HashtagEntity(string tag)
        {
            Tag = tag;
        }

        public HashtagEntity() { }
    }
}
