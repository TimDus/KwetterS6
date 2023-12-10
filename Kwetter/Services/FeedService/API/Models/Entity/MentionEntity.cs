using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FeedService.API.Models.Entity
{
    [Table("Mentions", Schema = "dbo")]
    public class MentionEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int KweetServiceId { get; set; }

        public CustomerEntity Customer { get; set; }

        public KweetEntity Kweet { get; set; }

        public MentionEntity() { }

        public MentionEntity(int kweetServiceId, CustomerEntity customer)
        {
            KweetServiceId = kweetServiceId;
            Customer = customer;
        }
    }
}
