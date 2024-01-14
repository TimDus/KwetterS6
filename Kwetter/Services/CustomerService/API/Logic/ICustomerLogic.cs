using CustomerService.API.Models.Auth;
using CustomerService.API.Models.DTO;
using CustomerService.API.Models.Entity;
using Microsoft.AspNetCore.Mvc;

namespace CustomerService.API.Logic
{
    public interface ICustomerLogic
    {
        Task<CustomerAuthDto> CreateCustomerLogic(CustomerAuthDto customerAuthDTO);

        Task<AuthResponse> LoginCustomerLogic(CustomerAuthDto customerAuthDTO, RefreshToken refreshToken);

        Task<CustomerEntity> GetCustomer(int id);

        Task<AuthResponse> CreateToken(CustomerEntity customer);

        Task SetRefreshToken(CustomerEntity customer);

        Task DeleteAccount(int id);
    }
}
