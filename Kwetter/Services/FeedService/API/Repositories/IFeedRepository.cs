using FeedService.API.Models.Entity;

namespace FeedService.API.Repositories
{
    public interface IFeedRepository
    {
        Task<KweetEntity> CreateKweet(KweetEntity obj);

        Task<KweetEntity> UpdateKweet(KweetEntity obj);

        Task<CustomerEntity> CreateCustomer(CustomerEntity obj);

        Task<KweetLikeEntity> LikeKweet(KweetLikeEntity obj);

        Task<KweetLikeEntity> UnlikeKweet(KweetLikeEntity obj);

        Task<FollowEntity> FollowCustomer(FollowEntity obj);

        Task<FollowEntity> UnfollowCustomer(FollowEntity obj);

        Task<CustomerEntity> GetCustomer(int id);

        Task<KweetEntity> GetKweet(int id);
    }
}
