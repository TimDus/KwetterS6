using FeedService.API.Models.Entity;
using FeedService.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FeedService.API.Repositories
{
    public class KweetLikeRepository : IKweetLikeRepository
    {
        private readonly FeedDbContext _feedDbContext;

        public KweetLikeRepository(FeedDbContext dbContext)
        {
            _feedDbContext = dbContext;
        }

        public async Task<KweetLikeEntity> Create(KweetLikeEntity obj)
        {
            await _feedDbContext.KweetLikes.AddAsync(obj);
            await _feedDbContext.SaveChangesAsync();

            return await _feedDbContext.KweetLikes.Where(a => a.Id == obj.Id).FirstOrDefaultAsync();
        }

        public async Task<KweetLikeEntity> Delete(int id)
        {
            KweetLikeEntity kweetLike = await _feedDbContext.KweetLikes.Where(kl => kl.KweetLikeServiceId == id).Include(kl => kl.Kweet).Include(kl => kl.Customer).FirstOrDefaultAsync();
            _feedDbContext.KweetLikes.Remove(kweetLike);
            await _feedDbContext.SaveChangesAsync();

            return kweetLike;
        }

        public async Task<KweetLikeEntity> GetById(int id)
        {
            return await _feedDbContext.KweetLikes.Where(kl => kl.KweetLikeServiceId == id).FirstOrDefaultAsync();
        }

        public Task<KweetLikeEntity> Update(KweetLikeEntity obj)
        {
            throw new NotImplementedException();
        }
    }
}
