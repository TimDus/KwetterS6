using FeedService.API.Models.Entity;

namespace FeedService.API.Repositories
{
    public class FeedRepository : IFeedRepository
    {
        private readonly FeedDbContext _dbContext;

        public FeedRepository(FeedDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async void AddKweet(KweetEntity kweet)
        {
            throw new NotImplementedException();
        }
    }
}
