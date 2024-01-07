using CustomerService.API.Logic;
using CustomerService.API.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerLogic _customerlogic;

        public CustomerController(ICustomerLogic customerLogic)
        {
            _customerlogic = customerLogic;
        }

        [HttpPost("create"), AllowAnonymous]
        public async Task<CustomerAuthDto> Create(CustomerAuthDto customerAuthDTO)
        {
            return await _customerlogic.CreateCustomerLogic(customerAuthDTO);
        }

        [HttpPost("login"), AllowAnonymous]
        public async Task<AuthResponse> Login(CustomerAuthDto customerAuthDTO)
        {
            return await _customerlogic.LoginCustomerLogic(customerAuthDTO);
        }

        [HttpPut("update"), Authorize(Roles = "Customer")]
        public async Task<ActionResult> Update(String customer)
        {
            await Task.CompletedTask;
            return Ok();
        }

        [HttpPut("updateadmin"), Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateAdmin(String customer)
        {
            await Task.CompletedTask;
            return Ok();
        }
    }
}