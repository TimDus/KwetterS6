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
        private readonly IMediator _mediator;
        public KweetController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public ActionResult Create(string kweet)
        {

            var command = new CreateKwetCommand
            {
                Id = Guid.NewGuid(),
                CreatedDate = DateTime.Now,
                Kweet = kweet
            };

            _mediator.Send(command);

            // Return a 200 OK response
            return Ok();
        }
    }
}