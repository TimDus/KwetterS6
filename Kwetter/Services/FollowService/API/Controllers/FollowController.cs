using FollowService.API.Logic;
using FollowService.API.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace FollowService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FollowController : ControllerBase
    {
        private readonly IFollowLogic _followLogic;

        public FollowController(IFollowLogic followLogic)
        {
            _followLogic = followLogic;
        }

        [HttpPost("Follow")]
        public async Task<ActionResult> Follow([FromBody] FollowDTO followDTO)
        {
            await _followLogic.CustomerFollowedLogic(followDTO);
            return Ok();
        }

        [HttpPost("Unfollow")]
        public async Task<ActionResult> Unfollow([FromBody] FollowDTO followDTO)
        {
            await _followLogic.CustomerUnfollowedLogic(followDTO);
            return Ok();
        }

        [HttpGet("GetFollowers")]
        public async Task<ActionResult<FollowListDTO>> GetFollowers(int customerId)
        {
            return await _followLogic.GetFollowersLogic(customerId);
        }

        [HttpGet("GetFollowing")]
        public async Task<ActionResult<FollowListDTO>> GetFollowing(int customerId)
        {
            return await _followLogic.GetFollowingLogic(customerId);
        }
    }
}