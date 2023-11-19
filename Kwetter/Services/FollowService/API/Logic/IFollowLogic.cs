using FollowService.API.Models.DTO;

namespace FollowService.API.Logic
{
    public interface IFollowLogic
    {
        public Task<FollowDTO> CustomerFollowedLogic(FollowDTO kweetDTO);

        public Task<FollowDTO> CustomerUnfollowedLogic(FollowDTO kweetDTO);

        public Task<CustomerDTO> GetFollowersLogic(CustomerDTO customerDTO);

        public Task<CustomerDTO> GetFollowingLogic(CustomerDTO customerDTO);
    }
}
