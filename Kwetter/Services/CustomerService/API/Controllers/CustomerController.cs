using Microsoft.AspNetCore.Mvc;

namespace CustomerService.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {

        [HttpPost("Create")]
        public ActionResult Create(String customer)
        {
            return Ok();
        }

        [HttpPut("Update")]
        public ActionResult Update(String customer)
        {   
            return Ok();
        }
    }
}