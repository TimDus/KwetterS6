using FeedService.API.Models.Entity;
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

        public async Task<CustomerEntity> CreateCustomer(CustomerEntity obj)
        {
            await _feedDbContext.Customers.AddAsync(obj);
            await _feedDbContext.SaveChangesAsync();

            return await _feedDbContext.Customers.Where(a => a.Id == obj.Id).FirstOrDefaultAsync();
        }

        public async Task<KweetEntity> CreateKweet(KweetEntity obj)
        {
            await _feedDbContext.Kweets.AddAsync(obj);
            await _feedDbContext.SaveChangesAsync();

            return await _feedDbContext.Kweets.Where(a => a.Id == obj.Id).FirstOrDefaultAsync();
        }

        public async Task<FollowEntity> FollowCustomer(FollowEntity obj)
        {
            await _feedDbContext.Follows.AddAsync(obj);
            await _feedDbContext.SaveChangesAsync();

            return await _feedDbContext.Follows.Where(a => a.Id == obj.Id).FirstOrDefaultAsync();
        }

        public async Task<CustomerEntity> GetCustomer(int id)
        {
            return await _feedDbContext.Customers.Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<FollowEntity> GetFollow(int id)
        {
            return await _feedDbContext.Follows.Where(f => f.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<KweetEntity>> GetFollowedKweetsFeed(int id)
        {
            return await _feedDbContext.Kweets.Where(k => k.Id == id).OrderByDescending(k => k.CreatedDate).Take(20).ToListAsync();
        }

        public async Task<List<KweetEntity>> GetFollowedKweetsFeedExtension(int id, DateTime time)
        {
            return await _feedDbContext.Kweets.Where(k => k.CreatedDate < time & k.Id == id).OrderByDescending(k => k.CreatedDate).Take(20).ToListAsync();
        }

        public async Task<KweetEntity> GetKweet(int id)
        {
            return await _feedDbContext.Kweets.Where(k => k.Id == id).FirstOrDefaultAsync();
        }

        public async Task<KweetLikeEntity> GetKweetLike(int id)
        {
            return await _feedDbContext.KweetLikes.Where(kl => kl.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<KweetEntity>> GetRandomKweetsFeed()
        {
            return await _feedDbContext.Kweets.OrderByDescending(k => k.CreatedDate).Take(20).ToListAsync();
        }

        public async Task<List<KweetEntity>> GetRandomKweetsFeedExtension(DateTime time)
        {
            return await _feedDbContext.Kweets.Where(k => k.CreatedDate < time).OrderByDescending(k => k.CreatedDate).Take(20).ToListAsync();
        }

        public async Task<KweetLikeEntity> LikeKweet(KweetLikeEntity obj)
        {
            await _feedDbContext.KweetLikes.AddAsync(obj);
            await _feedDbContext.SaveChangesAsync();

            return await _feedDbContext.KweetLikes.Where(a => a.Id == obj.Id).FirstOrDefaultAsync();
        }

        public async Task<FollowEntity> UnfollowCustomer(int id)
        {
            FollowEntity follow = await _feedDbContext.Follows.Where(f => f.Id == id).Include(f => f.Follower).Include(f => f.Following).FirstOrDefaultAsync();
            _feedDbContext.Follows.Remove(follow);
            await _feedDbContext.SaveChangesAsync();

            return follow;
        }

        public async Task<KweetLikeEntity> UnlikeKweet(int id)
        {
            KweetLikeEntity kweetLike = await _feedDbContext.KweetLikes.Where(kl => kl.KweetLikeServiceId == id).Include(kl => kl.Kweet).Include(kl => kl.Customer).FirstOrDefaultAsync();
            _feedDbContext.KweetLikes.Remove(kweetLike);
            await _feedDbContext.SaveChangesAsync();

            return kweetLike;
        }

        public Task<KweetEntity> UpdateKweet(KweetEntity obj)
        {
            throw new NotImplementedException();
        }
    }
}
