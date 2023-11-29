using Common.Interfaces;
using FollowService.API.Models.Entity;

namespace FollowService.API.Repositories
{
    public interface IFollowRepository : IGenericRepository<FollowEntity>
    {
        Task<List<FollowEntity>> GetFollowers(int accountId);

        Task<List<FollowEntity>> GetFollowing(int accountId);

        Task<CustomerEntity> CreateCustomer(CustomerEntity obj);

        Task<CustomerEntity> GetCustomer(int id);
    }
}
