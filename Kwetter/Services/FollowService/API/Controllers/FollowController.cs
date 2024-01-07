using FollowService.API.Logic;
using FollowService.API.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace FollowService.API.Controllers
{
    [ApiController]
    [Route("api/follow")]
    public class FollowController : ControllerBase
    {
        private readonly IFollowLogic _followLogic;

        public FollowController(IFollowLogic followLogic)
        {
            _followLogic = followLogic;
        }

        [HttpPost("follow")]
        public async Task<ActionResult<FollowDTO>> Follow([FromBody] FollowDTO followDTO)
        {
            return await _followLogic.CustomerFollowedLogic(followDTO);
        }

        [HttpPost("unfollow")]
        public async Task<ActionResult<FollowDTO>> Unfollow(int followId)
        {
            return await _followLogic.CustomerUnfollowedLogic(followId);
        }

        [HttpGet("getfollowers")]
        public async Task<ActionResult<FollowListDTO>> GetFollowers(int customerId)
        {
            return await _followLogic.GetFollowersLogic(customerId);
        }

        [HttpGet("getfollowing")]
        public async Task<ActionResult<FollowListDTO>> GetFollowing(int customerId)
        {
            return await _followLogic.GetFollowingLogic(customerId);
        }
    }
}