using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KweetService.API.Models.Entity
{
    public class MentionEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        
        [ForeignKey("customer")]
        [Column("customerid")]
        public int CustomerId { get; set; }

        [ForeignKey("kweet")]
        [Column("kweetid")]
        public int KweetId { get; set; }

    }
}
