using KweetService.API.EventPublisher.KweetCreated;
using KweetService.API.Models.DTO;
using KweetService.API.Models.Entity;
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

        public void CreateKweetLogic(KweetEntity kweetEntity)
        {
            var kweet = new KweetCreatedEvent
            {
                KweetCreatedDate = DateTime.Now,
                Text = kweetEntity.Text
            };

            _mediator.Send(kweet);
        }
    }
}
