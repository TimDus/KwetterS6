using KweetService.API.Logic;
using KweetService.API.Temp;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Web;

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
        public ActionResult Create(string kweet)
        {
            var kweetEntity = new KweetEntity
            {
                Kweet = kweet
            };

            _kweetLogic.CreateKweetLogic(kweetEntity);

            // Return a 200 OK response
            return Ok();
        }
    }
}