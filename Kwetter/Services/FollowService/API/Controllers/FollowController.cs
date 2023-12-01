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
        public async Task<ActionResult<FollowDTO>> Follow([FromBody] FollowDTO followDTO)
        {
            return await _followLogic.CustomerFollowedLogic(followDTO);
        }

        [HttpPost("Unfollow")]
        public async Task<ActionResult<FollowDTO>> Unfollow(int followId)
        {
            return await _followLogic.CustomerUnfollowedLogic(followId);
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