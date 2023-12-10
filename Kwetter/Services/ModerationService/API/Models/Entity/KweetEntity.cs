using ModerationService.API.Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModerationService.API.Models.Entity
{
    [Table("Kweets", Schema = "dbo")]
    public class KweetEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int KweetServiceId { get; set; }

        public CustomerEntity Customer { get; set; } = new CustomerEntity();

        public string Text { get; set; }

        public DateTime CreatedDate { get; set; }

        public Status Status { get; set; }

        public KweetEntity(int kweetServiceId, string text, DateTime dateTime, Status status)
        {
            KweetServiceId = kweetServiceId;
            Text = text;
            CreatedDate = dateTime;
            Status = status;
        }

        public KweetEntity() { }
    }
}
