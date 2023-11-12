using KweetService.API.Eventing.EventPublisher.KweetCreated;
using KweetService.API.Models.DTO;
using KweetService.API.Models.Entity;
using KweetService.API.Repositories;
using MediatR;

namespace KweetService.API.Logic
{
    public class KweetLogic : IKweetLogic
    {
        private readonly IMediator _mediator;
        private readonly IKweetRepository _repository;

        public KweetLogic(IMediator mediator, IKweetRepository repository)
        {
            _mediator = mediator;
            _repository = repository;
        }

        public async Task<KweetDTO> CreateKweetLogic(KweetDTO kweetDTO)
        {
            var kweetEntity = new KweetEntity
            {
                Text = kweetDTO.Text,
                CustomerId = kweetDTO.Customer.Id,
                CreatedDate = DateTime.Now
            };

            kweetEntity = await _repository.Create(kweetEntity);

            var kweet = new KweetCreatedEvent
            {
                CustomerId = (long)kweetEntity.CustomerId,
                Text = kweetEntity.Text,
                KweetCreatedDate = kweetEntity.CreatedDate
            };

            await _mediator.Send(kweet);

            return kweetDTO;
        }

        public async Task<KweetDTO> LikeKweetLogic(KweetDTO kweet)
        {
            throw new NotImplementedException();
        }

        public async Task<KweetDTO> UnlikeKweetLogic(KweetDTO kweet)
        {
            throw new NotImplementedException();
        }
    }
}
