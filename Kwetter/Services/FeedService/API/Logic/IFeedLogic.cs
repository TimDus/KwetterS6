using FeedService.API.Models.DTO;

namespace FeedService.API.Logic
{
    public interface IFeedLogic
    {
        Task<List<KweetDTO>> GetRandomKweetsFeed(int customerId);

        Task<List<KweetDTO>> GetRandomKweetsFeedExtension(int customerId, DateTime time);

        Task<List<KweetDTO>> GetFollowedKweetsFeed(int customerId);

        Task<List<KweetDTO>> GetFollowedKweetsFeedExtension(int customerId, DateTime time);
    }
}
