using FeedService.API.Models.Entity;

namespace FeedService.API.Repositories
{
    public interface IFeedRepository
    {
        Task<KweetEntity> CreateKweet(KweetEntity obj);

        Task<KweetEntity> UpdateKweet(KweetEntity obj);

        Task<CustomerEntity> CreateCustomer(CustomerEntity obj);

        Task<KweetLikeEntity> LikeKweet(KweetLikeEntity obj);

        Task<KweetLikeEntity> UnlikeKweet(int id);

        Task<FollowEntity> FollowCustomer(FollowEntity obj);

        Task<FollowEntity> UnfollowCustomer(int id);

        Task<CustomerEntity> GetCustomer(int id);

        Task<KweetEntity> GetKweet(int id);

        Task<KweetLikeEntity> GetKweetLike(int id);

        Task<FollowEntity> GetFollow(int id);

        Task<List<KweetEntity>> GetRandomKweetsFeed();

        Task<List<KweetEntity>> GetRandomKweetsFeedExtension(DateTime time);

        Task<List<KweetEntity>> GetFollowedKweetsFeed(int id);

        Task<List<KweetEntity>> GetFollowedKweetsFeedExtension(int id, DateTime time);
    }
}
