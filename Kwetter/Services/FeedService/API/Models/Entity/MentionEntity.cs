using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FeedService.API.Models.Entity
{
    public class MentionEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int MentionServiceId { get; set; }

        public CustomerEntity Customer { get; set; }

        public KweetEntity Kweet { get; set; }

        public MentionEntity() { }

        public MentionEntity(int mentionServiceId, CustomerEntity customer, KweetEntity kweet)
        {
            MentionServiceId = mentionServiceId;
            Customer = customer;
            Kweet = kweet;
        }
    }
}
