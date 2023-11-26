namespace CustomerService.API.Models.DTO
{
    public class CustomerDTO
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public string DisplayName { get; set; }

        public string CustomerName { get; set; }

        public string? ProfilePicture { get; set; }

        public string? ProfileBio { get; set; }
    }
}
