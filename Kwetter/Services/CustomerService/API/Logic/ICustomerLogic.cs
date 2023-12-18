using CustomerService.API.Models.DTO;

namespace CustomerService.API.Logic
{
    public interface ICustomerLogic
    {
        Task<CustomerCreateDTO> CreateCustomerLogic(CustomerCreateDTO customerDTO);
    }
}
