namespace CustomerService.API.Models.DTO
{
    public class CustomerCreateDTO
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public string DisplayName { get; set; }

        public string CustomerName { get; set; }
    }
}
