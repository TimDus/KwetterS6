using Microsoft.AspNetCore.Mvc;
using ModerationService.API.Logic;
using ModerationService.API.Models.DTO;

namespace ModerationService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ModerationController : ControllerBase
    {

        private readonly IModerationLogic _moderationLogic;

        public ModerationController(IModerationLogic moderationLogic)
        {
            _moderationLogic = moderationLogic;
        }

        [HttpGet("GetPendingList")]
        public async Task<List<KweetDTO>> GetPendingList()
        {
            return await _moderationLogic.GetPendingList();
        }

        [HttpPost("CheckKweet")]
        public async Task<KweetDTO> CheckKweet([FromBody] KweetDTO kweetDTO)
        {
            return await _moderationLogic.CheckKweet(kweetDTO);
        }
    }
}