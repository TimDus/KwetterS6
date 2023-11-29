using CustomerService.API.Logic;
using CustomerService.API.Models.DTO;
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

        [HttpPost("Create")]
        public async Task<CustomerCreateDTO> Create(CustomerCreateDTO customerDTO)
        {
            return await _customerlogic.CreateCustomerLogic(customerDTO);
        }

        [HttpPut("Update")]
        public async Task<ActionResult> Update(String customer)
        {   
            return Ok();
        }
    }
}