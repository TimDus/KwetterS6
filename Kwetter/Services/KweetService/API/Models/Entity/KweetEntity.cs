using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KweetService.API.Models.Entity
{
    public class KweetEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }

        [ForeignKey("customer")]
        [Column("customerid")]
        public long CustomerId { get; set; }

        [Column("text")]
        public string Text { get; set; }

        [Column("posted")]
        public DateTime CreatedDate { get; set; }
    }
}
