using FollowService.API.Models.Entity;

namespace FollowService.API.Models.DTO
{
    public class FollowListDTO
    {
        public ICollection<FollowEntity> Followers { get; set; }

        public ICollection<FollowEntity> Following { get; set; }
    }
}
