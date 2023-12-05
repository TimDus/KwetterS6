using FeedService.API.Models.Entity;
using FeedService.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FeedService.API.Repositories
{
    public class FeedRepository : IFeedRepository
    {
        private readonly FeedDbContext _feedDbContext;

        public FeedRepository(FeedDbContext dbContext) 
        {
            _feedDbContext = dbContext;
        }

        public async Task<List<FollowEntity>> GetFollowedKweetsFeed(int id)
        {
            return await _feedDbContext.Follows
                .Where(f => f.Follower.Id == id)
                .Include(f => f.Following)
                .Include(f => f.Following.Kweets.OrderByDescending(f => f.CreatedDate).Take(5))
                .ThenInclude(k => k.Mentions)
                .Include(f => f.Following.Kweets.OrderByDescending(f => f.CreatedDate).Take(5))
                .ThenInclude(k => k.Hashtags)
                .Include(f => f.Following.Kweets.OrderByDescending(f => f.CreatedDate).Take(5))
                .ThenInclude(k => k.Likes.Where(kl => kl.Customer.CustomerId == id))
                .OrderByDescending(f => f.Following.Kweets.First().CreatedDate).Take(5)
                .ToListAsync();
        }

        public async Task<List<FollowEntity>> GetFollowedKweetsFeedExtension(int id, DateTime time)
        {
            return await _feedDbContext.Follows
                .Where(f => f.Follower.Id == id)
                .Include(f => f.Following)
                .Include(f => f.Following.Kweets.Where(k => k.CreatedDate < time).OrderByDescending(k => k.CreatedDate).Take(5))
                .ThenInclude(k => k.Mentions)
                .Include(f => f.Following.Kweets.Where(k => k.CreatedDate < time).OrderByDescending(k => k.CreatedDate).Take(5))
                .ThenInclude(k => k.Hashtags)
                .Include(f => f.Following.Kweets.Where(k => k.CreatedDate < time).OrderByDescending(k => k.CreatedDate).Take(5))
                .ThenInclude(k => k.Likes.Where(kl => kl.Customer.CustomerId == id))
                .OrderByDescending(f => f.Following.Kweets.First().CreatedDate).Take(5)
                .ToListAsync();
        }

        public async Task<List<CustomerEntity>> GetRandomKweetsFeed(int id)
        {
            return await _feedDbContext.Customers
                .Where(c => c.Kweets != null)
                .Include(c => c.Kweets.OrderByDescending(k => k.CreatedDate).Take(5))
                .ThenInclude(k => k.Mentions)
                .Include(c => c.Kweets.OrderByDescending(k => k.CreatedDate).Take(5))
                .ThenInclude(k => k.Hashtags)
                .Include(c => c.Kweets.OrderByDescending(k => k.CreatedDate).Take(5))
                .ThenInclude(k => k.Likes.Where(kl => kl.Customer.CustomerId == id))
                .OrderByDescending(c => c.Kweets.First().CreatedDate).Take(5)
                .ToListAsync();
        }

        public async Task<List<CustomerEntity>> GetRandomKweetsFeedExtension(int id, DateTime time)
        {
            return await _feedDbContext.Customers
                .Where(c => c.Kweets != null)
                .Include(c => c.Kweets.Where(k => k.CreatedDate < time).OrderByDescending(k => k.CreatedDate).Take(5))
                .ThenInclude(k => k.Mentions)
                .Include(c => c.Kweets.Where(k => k.CreatedDate < time).OrderByDescending(k => k.CreatedDate).Take(5))
                .ThenInclude(k => k.Hashtags)
                .Include(c => c.Kweets.Where(k => k.CreatedDate < time).OrderByDescending(k => k.CreatedDate).Take(5))
                .ThenInclude(k => k.Likes.Where(kl => kl.Customer.CustomerId == id))
                .OrderByDescending(c => c.Kweets.First().CreatedDate).Take(5)
                .ToListAsync();
        }
    }
}
