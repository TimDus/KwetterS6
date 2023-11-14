using Microsoft.AspNetCore.Mvc;

namespace FollowService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FollowController : ControllerBase
    {
        [HttpPost("Follow")]
        public ActionResult Follow(string Follow)
        {
            return Ok();
        }

        [HttpPost("Unfollow")]
        public ActionResult Unfollow(string Follow)
        {
            return Ok();
        }

        [HttpGet]
        public ActionResult GetFollowers(string Follow)
        {
            return Ok();
        }
    }
}