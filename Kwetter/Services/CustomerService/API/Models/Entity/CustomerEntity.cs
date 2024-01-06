using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerService.API.Models.Entity
{
    [Table("Customers", Schema = "dbo")]
    public class CustomerEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string DisplayName { get; set; }

        public string CustomerName { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public string Role { get; set; }

        public string? ProfilePicture { get; set; }

        public string? ProfileBio { get; set; }

        public CustomerEntity() { }

        public CustomerEntity(string displayName, string customerName, byte[] passwordHash, byte[] passwordSalt)
        {
            DisplayName = displayName;
            CustomerName = customerName;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
            Role = "Customer";
        }
    }
}
