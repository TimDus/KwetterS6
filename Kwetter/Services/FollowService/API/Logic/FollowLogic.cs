using AutoMapper;
using FollowService.API.Eventing.EventPublisher.CustomerFollowed;
using FollowService.API.Eventing.EventPublisher.CustomerUnfollowed;
using FollowService.API.Models.DTO;
using FollowService.API.Models.Entity;
using FollowService.API.Repositories;
using MediatR;

namespace FollowService.API.Logic
{
    public class FollowLogic : IFollowLogic
    {
        private readonly IMediator _mediator;
        private readonly IFollowRepository _repository;
        private readonly IMapper _mapper;

        public FollowLogic(IMediator mediator, IFollowRepository repository, IMapper mapper)
        {
            _mediator = mediator;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<FollowDTO> CustomerFollowedLogic(FollowDTO kweetDTO)
        {
            FollowEntity followEntity = _mapper.Map<FollowEntity>(kweetDTO);

            followEntity = await _repository.Create(followEntity);

            var follow = new CustomerFollowedEvent
            {
                FollowerId = followEntity.FollowerId,
                FollowingId = followEntity.FollowingId,
                FollowedDateTime = followEntity.FollowedDateTime
            };

            await _mediator.Send(follow);

            return kweetDTO;
        }

        public async Task<FollowDTO> CustomerUnfollowedLogic(FollowDTO kweetDTO)
        {
            FollowEntity followEntity = _mapper.Map<FollowEntity>(kweetDTO);

            await _repository.Delete(followEntity);

            var unfollow = new CustomerUnfollowedEvent
            {
                FollowerId = followEntity.FollowerId,
                FollowingId = followEntity.FollowingId,
            };

            await _mediator.Send(unfollow);

            return kweetDTO;
        }

        public async Task<CustomerDTO> GetFollowersLogic(CustomerDTO customerDTO)
        {
            CustomerEntity customerEntity = _mapper.Map<CustomerEntity>(customerDTO);

            customerEntity.Followers = await _repository.GetFollowers(customerEntity.Id);

            return _mapper.Map<CustomerDTO>(customerEntity);
        }

        public async Task<CustomerDTO> GetFollowingLogic(CustomerDTO customerDTO)
        {
            CustomerEntity customerEntity = _mapper.Map<CustomerEntity>(customerDTO);

            customerEntity.Following = await _repository.GetFollowing(customerEntity.Id);

            return _mapper.Map<CustomerDTO>(customerEntity);
        }
    }
}
