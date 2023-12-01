﻿using FeedService.API.Logic;
using FeedService.API.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace FeedService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedController : ControllerBase
    {
        private readonly IFeedLogic _feedLogic;

        public FeedController(IFeedLogic feedLogic)
        {
            _feedLogic = feedLogic;
        }

        [HttpGet("Random")]
        public async Task<List<KweetDTO>> Random()
        {
            return await _feedLogic.GetRandomKweetsFeed();
        }

        [HttpGet("RandomExtension")]
        public async Task<List<KweetDTO>> RandomExtension(DateTime time)
        {
            return await _feedLogic.GetRandomKweetsFeedExtension(time);
        }

        [HttpGet("Followed")]
        public async Task<List<KweetDTO>> Followed(int customerId)
        {
            return await _feedLogic.GetFollowedKweetsFeed(customerId);
        }

        [HttpGet("FollowedExtension")]
        public async Task<List<KweetDTO>> FollowedExtension(int customerId, DateTime time)
        {
            return await _feedLogic.GetFollowedKweetsFeedExtension(customerId, time);
        }
    }
}
