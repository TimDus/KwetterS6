using KweetService.API.Temp;
using MediatR;

namespace KweetService.API.Logic
{
    public class KweetLogic : IKweetLogic
    {
        private readonly IMediator _mediator;
        public KweetLogic(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void CreateKweetLogic(KweetEntity kweet)
        {
            var command = new CreateKweetCommand
            {
                Id = Guid.NewGuid(),
                CreatedDate = DateTime.Now,
                Kweet = kweet.Kweet
            };

            _mediator.Send(command);
        }
    }
}
