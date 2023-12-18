using MediatR;
using ModerationService.API.Models.DTO;
using ModerationService.API.Models.Entity;
using ModerationService.API.Repositories;

namespace ModerationService.API.Logic
{
    public class ModerationLogic : IModerationLogic
    {
        //private readonly IMediator _mediator;
        private readonly IModerationRepository _repository;

        public ModerationLogic(IMediator mediator, IModerationRepository repository)
        {
            //_mediator = mediator;
            _repository = repository;
        }

        public Task<KweetDTO> CheckKweet(KweetDTO kweetDTO)
        {
            throw new NotImplementedException();
        }

        public async Task<List<KweetDTO>> GetPendingList()
        {
            List<KweetEntity> kweetEntities = await _repository.GetPendingList();

            List<KweetDTO> kweets = new();

            foreach(KweetEntity kweet in kweetEntities)
            {
                kweets.Add(new(kweet.KweetServiceId, kweet.Customer.Id, kweet.Customer.CustomerName, kweet.Text, kweet.CreatedDate));
            }

            return kweets;
        }
    }
}
