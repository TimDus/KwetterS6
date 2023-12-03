using FeedService.API.Models.Entity;
using FeedService.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FeedService.API.Repositories
{
    public class FollowRepository : IFollowRepository
    {
        private readonly FeedDbContext _feedDbContext;

        public FollowRepository(FeedDbContext dbContext)
        {
            _feedDbContext = dbContext;
        }

        public async Task<FollowEntity> Create(FollowEntity obj)
        {
            await _feedDbContext.Follows.AddAsync(obj);
            await _feedDbContext.SaveChangesAsync();

            return await _feedDbContext.Follows.Where(a => a.Id == obj.Id).FirstOrDefaultAsync();
        }

        public async Task<FollowEntity> Delete(int id)
        {
            FollowEntity follow = await _feedDbContext.Follows.Where(f => f.FollowServiceId == id).Include(f => f.Follower).Include(f => f.Following).FirstOrDefaultAsync();
            _feedDbContext.Follows.Remove(follow);
            await _feedDbContext.SaveChangesAsync();

            return follow;
        }

        public async Task<FollowEntity> GetById(int id)
        {
            return await _feedDbContext.Follows.Where(f => f.FollowServiceId == id).FirstOrDefaultAsync();
        }

        public Task<FollowEntity> Update(FollowEntity obj)
        {
            throw new NotImplementedException();
        }
    }
}
