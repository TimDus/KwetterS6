using Microsoft.AspNetCore.Mvc;
using ModerationService.API.Logic;
using ModerationService.API.Models.DTO;

namespace ModerationService.API.Controllers
{
    [ApiController]
    [Route("api/moderation")]
    public class ModerationController : ControllerBase
    {

        private readonly IModerationLogic _moderationLogic;

        public ModerationController(IModerationLogic moderationLogic)
        {
            _moderationLogic = moderationLogic;
        }

        [HttpGet("getpendinglist")]
        public async Task<List<KweetDTO>> GetPendingList()
        {
            return await _moderationLogic.GetPendingList();
        }

        [HttpPost("checkkweet")]
        public async Task<string> CheckKweet([FromBody] KweetDTO kweetDTO)
        {
            return await _moderationLogic.CheckKweet(kweetDTO);
        }
    }
}