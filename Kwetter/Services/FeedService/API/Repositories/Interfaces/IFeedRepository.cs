using FeedService.API.Models.Entity;

namespace FeedService.API.Repositories.Interfaces
{
    public interface IFeedRepository
    {
        Task<List<CustomerEntity>> GetRandomKweetsFeed(int id);

        Task<List<CustomerEntity>> GetRandomKweetsFeedExtension(int id, DateTime time);

        Task<List<FollowEntity>> GetFollowedKweetsFeed(int id);

        Task<List<FollowEntity>> GetFollowedKweetsFeedExtension(int id, DateTime time);
    }
}
