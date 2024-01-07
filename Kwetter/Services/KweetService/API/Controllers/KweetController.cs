using KweetService.API.Logic;
using KweetService.API.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace KweetService.API.Controllers
{
    [ApiController]
    [Route("api/kweet")]
    public class KweetController : ControllerBase
    {
        private readonly IKweetLogic _kweetLogic;

        public KweetController(IKweetLogic kweetLogic)
        {
            _kweetLogic = kweetLogic;
        }

        [HttpPost("create")]
        public async Task<KweetCreatedDTO> Create([FromBody] KweetCreatedDTO kweetDTO)
        {
            return await _kweetLogic.CreateKweetLogic(kweetDTO);
        }

        [HttpPost("like")]
        public async Task<ActionResult> Like([FromBody] KweetLikeDTO kweetLikeDTO)
        {
            await _kweetLogic.LikeKweetLogic(kweetLikeDTO);

            // Return a 200 OK response
            return Ok();
        }

        [HttpPost("unlike")]
        public async Task<ActionResult> Unlike([FromBody] KweetLikeDTO kweetLikeDTO)
        {
            await _kweetLogic.UnlikeKweetLogic(kweetLikeDTO);

            // Return a 200 OK response
            return Ok();
        }
    }
}