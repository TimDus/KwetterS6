using FeedService.API.Logic;
using FeedService.API.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace FeedService.API.Controllers
{
    [ApiController]
    [Route("api/feed")]
    public class FeedController : ControllerBase
    {
        private readonly IFeedLogic _feedLogic;

        public FeedController(IFeedLogic feedLogic)
        {
            _feedLogic = feedLogic;
        }

        [HttpGet("random")]
        public async Task<List<KweetDTO>> Random(int id)
        {
            return await _feedLogic.GetRandomKweetsFeed(id);
        }

        [HttpGet("randomextension")]
        public async Task<List<KweetDTO>> RandomExtension(int id, DateTime time)
        {
            return await _feedLogic.GetRandomKweetsFeedExtension(id, time);
        }

        [HttpGet("followed")]
        public async Task<List<KweetDTO>> Followed(int customerId)
        {
            return await _feedLogic.GetFollowedKweetsFeed(customerId);
        }

        [HttpGet("followedextension")]
        public async Task<List<KweetDTO>> FollowedExtension(int customerId, DateTime time)
        {
            return await _feedLogic.GetFollowedKweetsFeedExtension(customerId, time);
        }
    }
}
