using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KweetService.API.Models.Entity
{
    public class CustomerEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("accountid")]
        public int CustomerId { get; set; }

        public ICollection<KweetEntity>? Kweets { get; set; }

        public ICollection<KweetLikeEntity>? Likes { get; set; }

        public CustomerEntity(int customerId)
        {
            CustomerId = customerId;
        }
    }
}
