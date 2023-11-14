using FeedService.API.Models.Entity;

namespace FeedService.API.Repositories
{
    public interface IFeedRepository
    {
        void AddKweet(KweetEntity kweet);
    }
}
