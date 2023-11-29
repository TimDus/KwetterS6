using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CustomerService.API.Models.Entity
{
    [Table("Customer", Schema = "dbo")]
    public class CustomerEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int AccountId { get; set; }

        public string DisplayName { get; set; }

        public string CustomerName { get; set; }

        public string? ProfilePicture { get; set; }

        public string? ProfileBio { get; set; }

        public CustomerEntity() { }

        public CustomerEntity(int accountId, string displayName, string customerName)
        {
            AccountId = accountId;
            DisplayName = displayName;
            CustomerName = customerName;
        }
    }
}
