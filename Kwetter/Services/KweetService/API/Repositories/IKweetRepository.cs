using Common.Interfaces;
using KweetService.API.Models.Entity;
using Microsoft.AspNetCore.Mvc;

namespace KweetService.API.Repositories
{
    public interface IKweetRepository : IGenericRepository<KweetEntity>
    {
        Task LikeKweet(int id);

        Task UnlikeKweet(int id);

        Task<CustomerEntity> AddCustomer(CustomerEntity obj);
    }
}
