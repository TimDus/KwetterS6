using Microsoft.AspNetCore.Mvc;

namespace CustomerService.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {

        [HttpPost]
        public ActionResult Create(String customer)
        {
            return Ok();
        }
    }
}