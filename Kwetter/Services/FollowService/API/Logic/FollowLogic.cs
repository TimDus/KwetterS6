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

        public FollowLogic(IMediator mediator, IFollowRepository repository)
        {
            _mediator = mediator;
            _repository = repository;
        }

        public async Task<FollowDTO> CustomerFollowedLogic(FollowDTO kweetDTO)
        {
            FollowEntity followEntity = new()
            {
                Following = await _repository.GetCustomer(kweetDTO.FollowingId),
                Follower = await _repository.GetCustomer(kweetDTO.FollowerId)
            };

            followEntity = await _repository.Create(followEntity);

            var follow = new CustomerFollowedEvent
            {
                FollowerId = followEntity.Follower.Id,
                FollowingId = followEntity.Following.Id,
                FollowedDateTime = followEntity.FollowedDateTime
            };

            await _mediator.Send(follow);

            return kweetDTO;
        }

        public async Task<FollowDTO> CustomerUnfollowedLogic(FollowDTO kweetDTO)
        {
            FollowEntity followEntity = new()
            {
                Following = await _repository.GetCustomer(kweetDTO.FollowingId),
                Follower = await _repository.GetCustomer(kweetDTO.FollowerId)
            };

            await _repository.Delete(followEntity);

            var unfollow = new CustomerUnfollowedEvent
            {
                FollowerId = followEntity.Follower.Id,
                FollowingId = followEntity.Following.Id,
            };

            await _mediator.Send(unfollow);

            return kweetDTO;
        }

        public async Task<FollowListDTO> GetFollowersLogic(int customerId)
        {
            FollowListDTO followList = new()
            {
                Followers = await _repository.GetFollowers(customerId)
            };

            
            return followList;
        }

        public async Task<FollowListDTO> GetFollowingLogic(int customerId)
        {
            FollowListDTO followList = new()
            {
                Following = await _repository.GetFollowers(customerId)
            };

            return followList;
        }
    }
}
