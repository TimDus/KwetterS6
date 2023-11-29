using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FeedService.API.Models.Entity
{
    public class MentionEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public CustomerEntity Customer { get; set; }

        public KweetEntity Kweet { get; set; }
    }
}
