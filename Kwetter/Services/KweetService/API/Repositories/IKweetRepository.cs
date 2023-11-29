using Common.Interfaces;
using KweetService.API.Models.DTO;
using KweetService.API.Models.Entity;
using Microsoft.AspNetCore.Mvc;

namespace KweetService.API.Repositories
{
    public interface IKweetRepository : IGenericRepository<KweetEntity>
    {
        Task<KweetLikeEntity> LikeKweet(KweetLikeEntity obj);

        Task<KweetLikeEntity> UnlikeKweet(KweetLikeEntity obj);

        Task<CustomerEntity> CreateCustomer(CustomerEntity obj);

        Task<CustomerEntity> GetCustomer(int id);

        Task<KweetLikeEntity> GetKweetLike(int kweetId, int customerId);
    }
}
