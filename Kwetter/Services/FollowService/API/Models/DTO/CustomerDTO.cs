using FollowService.API.Models.Entity;

namespace FollowService.API.Models.DTO
{
    public class CustomerDTO
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public string DisplayName { get; set; }

        public string CustomerName { get; set; }

        public string CustomerProfilePicture { get; set; }

        public ICollection<FollowEntity> Followers { get; set; }

        public ICollection<FollowEntity> Following { get; set; }
    }
}
