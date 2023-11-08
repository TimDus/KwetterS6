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

        [HttpPost]
        public ActionResult Create([FromBody] KweetDTO kweetDTO)
        {
            var kweetEntity = new KweetEntity
            {
                Text = kweetDTO.Text,
                CustomerId = kweetDTO.CustomerId,
                CreatedDate = kweetDTO.CreatedDate
            };

            _kweetLogic.CreateKweetLogic(kweetEntity);

            // Return a 200 OK response
            return Ok();
        }
    }
}