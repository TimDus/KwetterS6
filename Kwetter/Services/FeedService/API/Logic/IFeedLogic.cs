using FeedService.API.Models.DTO;

namespace FeedService.API.Logic
{
    public interface IFeedLogic
    {
        Task<List<KweetDTO>> GetRandomKweetsFeed();

        Task<List<KweetDTO>> GetRandomKweetsFeedExtension(DateTime time);

        Task<List<KweetDTO>> GetFollowedKweetsFeed(int customerId);

        Task<List<KweetDTO>> GetFollowedKweetsFeedExtension(int customerId, DateTime time);
    }
}
