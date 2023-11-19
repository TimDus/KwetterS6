using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KweetService.API.Models.Entity
{
    [Table("Hashtag", Schema = "dbo")]
    public class HashtagEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public KweetEntity Kweet { get; set; }

        public string Tag { get; set; }
    }
}
