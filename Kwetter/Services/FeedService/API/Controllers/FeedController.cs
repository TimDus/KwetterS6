using Microsoft.AspNetCore.Mvc;

namespace FeedService.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FeedController : ControllerBase
    {

        [HttpGet("Random")]
        public ActionResult Random(String customer)
        {
            return Ok();
        }

        [HttpGet("Followed")]
        public ActionResult Followed(String customer)
        {
            return Ok();
        }
    }
}
