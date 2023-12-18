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

        public async Task<FollowDTO> CustomerFollowedLogic(FollowDTO followDTO)
        {
            FollowEntity followEntity = new(
                await _repository.GetCustomer(followDTO.FollowerId),
                await _repository.GetCustomer(followDTO.FollowingId),
                DateTime.Now);

            followEntity = await _repository.Create(followEntity);

            var follow = new CustomerFollowedEvent
            {
                FollowServiceId = followEntity.Id,
                FollowerId = followEntity.Follower.Id,
                FollowingId = followEntity.Following.Id,
                FollowedDateTime = followEntity.FollowedDateTime
            };

            await _mediator.Send(follow);

            return followDTO;
        }

        public async Task<FollowDTO> CustomerUnfollowedLogic(int followId)
        {
            FollowEntity followEntity = await _repository.Delete(followId);

            CustomerUnfollowedEvent unfollow = new()
            {
                FollowServiceId = followId,
                FollowerId = followEntity.Follower.Id,
                FollowingId = followEntity.Following.Id,
            };

            await _mediator.Send(unfollow);

            FollowDTO followDTO = new()
            {
                FollowerId = followEntity.Follower.Id,
                FollowingId = followEntity.Following.Id
            };

            return followDTO;
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
