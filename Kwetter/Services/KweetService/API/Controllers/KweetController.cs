using KweetService.API.Logic;
using KweetService.API.Models.DTO;
using KweetService.API.Models.Entity;
using Microsoft.AspNetCore.Mvc;

namespace KweetService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KweetController : ControllerBase
    {
        private readonly IKweetLogic _kweetLogic;

        public KweetController(IKweetLogic kweetLogic)
        {
            _kweetLogic = kweetLogic;
        }

        [HttpPost("Create")]
        public async Task<KweetCreateDTO> Create([FromBody] KweetCreateDTO kweetDTO)
        {
            return await _kweetLogic.CreateKweetLogic(kweetDTO);
        }

        [HttpPost("Like")]
        public async Task<ActionResult> Like([FromBody] KweetLikeDTO kweetLikeDTO)
        {
            await _kweetLogic.LikeKweetLogic(kweetLikeDTO);

            // Return a 200 OK response
            return Ok();
        }

        [HttpPost("Unlike")]
        public async Task<ActionResult> Unlike([FromBody] KweetLikeDTO kweetLikeDTO)
        {
            await _kweetLogic.UnlikeKweetLogic(kweetLikeDTO);

            // Return a 200 OK response
            return Ok();
        }
    }
}