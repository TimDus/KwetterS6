using FollowService.API.Models.DTO;

namespace FollowService.API.Logic
{
    public interface IFollowLogic
    {
        public Task<FollowDTO> CustomerFollowedLogic(FollowDTO kweetDTO);

        public Task<FollowDTO> CustomerUnfollowedLogic(FollowDTO kweetDTO);

        public Task<FollowListDTO> GetFollowersLogic(int customerId);

        public Task<FollowListDTO> GetFollowingLogic(int customerId);
    }
}
