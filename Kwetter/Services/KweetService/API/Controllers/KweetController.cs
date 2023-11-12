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
        public async Task<ActionResult> Create([FromBody] KweetDTO kweetDTO)
        {
            await _kweetLogic.CreateKweetLogic(kweetDTO);

            // Return a 200 OK response
            return Ok();
        }

        [HttpPost("Like")]
        public ActionResult Like([FromBody] KweetDTO kweetDTO)
        {

            // Return a 200 OK response
            return Ok();
        }

        [HttpPost("Unlike")]
        public ActionResult Unlike([FromBody] KweetDTO kweetDTO)
        {

            // Return a 200 OK response
            return Ok();
        }
    }
}