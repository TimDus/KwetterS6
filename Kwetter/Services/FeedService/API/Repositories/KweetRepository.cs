using FeedService.API.Models.Entity;
using FeedService.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FeedService.API.Repositories
{
    public class KweetRepository : IKweetRepository
    {
        private readonly FeedDbContext _feedDbContext;

        public KweetRepository(FeedDbContext dbContext)
        {
            _feedDbContext = dbContext;
        }

        public async Task<KweetEntity> Create(KweetEntity obj)
        {
            await _feedDbContext.Kweets.AddAsync(obj);
            await _feedDbContext.SaveChangesAsync();

            return await _feedDbContext.Kweets.Where(a => a.Id == obj.Id).FirstOrDefaultAsync();
        }

        public Task<KweetEntity> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<KweetEntity> GetById(int id)
        {
            return await _feedDbContext.Kweets.Where(k => k.KweetServiceId == id).FirstOrDefaultAsync();
        }

        public Task<KweetEntity> Update(KweetEntity obj)
        {
            throw new NotImplementedException();
        }
    }
}
