using CustomerService.API.Models.DTO;

namespace CustomerService.API.Logic
{
    public interface ICustomerLogic
    {
        Task<CustomerAuthDto> CreateCustomerLogic(CustomerAuthDto customerAuthDTO);

        Task<AuthResponse> LoginCustomerLogic(CustomerAuthDto customerAuthDTO);
    }
}
