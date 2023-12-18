using FollowService.API.Models.DTO;

namespace FollowService.API.Logic
{
    public interface IFollowLogic
    {
        public Task<FollowDTO> CustomerFollowedLogic(FollowDTO followDTO);

        public Task<FollowDTO> CustomerUnfollowedLogic(int followId);

        public Task<FollowListDTO> GetFollowersLogic(int customerId);

        public Task<FollowListDTO> GetFollowingLogic(int customerId);
    }
}
