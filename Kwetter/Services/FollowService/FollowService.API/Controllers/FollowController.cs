using Microsoft.AspNetCore.Mvc;

namespace FollowService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FollowController : ControllerBase
    {
        [HttpGet]
        public ActionResult Get(string Follow)
        {
            return Ok();
        }
    }
}