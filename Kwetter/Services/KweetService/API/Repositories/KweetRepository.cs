using KweetService.API.Models.DTO;
using KweetService.API.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KweetService.API.Repositories
{
    public class KweetRepository : IKweetRepository
    {
        private readonly KweetDbContext _kweetDbContext;

        public KweetRepository(KweetDbContext kweetDbContext)
        {
            _kweetDbContext = kweetDbContext;
        }

        public async Task<CustomerEntity> CreateCustomer(CustomerEntity obj)
        {
            await _kweetDbContext.Customers.AddAsync(obj);
            await _kweetDbContext.SaveChangesAsync();

            return await _kweetDbContext.Customers.Where(a => a.Id == obj.Id).FirstOrDefaultAsync();
        }

        public async Task<KweetEntity> Create(KweetEntity obj)
        {
            await _kweetDbContext.Kweets.AddAsync(obj);
            await _kweetDbContext.SaveChangesAsync();

            return await _kweetDbContext.Kweets
                .Where(k => k.Id == obj.Id)
                .Include(k => k.Mentions)
                .Include(k => k.Hashtags)
                .FirstOrDefaultAsync();
        }

        public async Task<KweetEntity> Delete(int id)
        {
            KweetEntity kweet = await _kweetDbContext.Kweets.Where(a => a.Id == id).FirstOrDefaultAsync();
            _kweetDbContext.Kweets.Remove(kweet);
            await _kweetDbContext.SaveChangesAsync();

            return kweet;
        }

        public async Task<KweetEntity> GetById(int id)
        {
            return await _kweetDbContext.Kweets.Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<CustomerEntity> GetCustomer(int id)
        {
            return await _kweetDbContext.Customers.Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<KweetLikeEntity> LikeKweet(KweetLikeEntity obj)
        {
            await _kweetDbContext.KweetLikes.AddAsync(obj);
            await _kweetDbContext.SaveChangesAsync();
            
            return await _kweetDbContext.KweetLikes.Where(a => a.Id == obj.Id).FirstOrDefaultAsync();
        }

        public async Task<KweetLikeEntity> UnlikeKweet(KweetLikeEntity obj)
        {
            KweetLikeEntity kweetLikeEntity = await _kweetDbContext.KweetLikes.Where(a => a.Id == obj.Id).FirstOrDefaultAsync();
            _kweetDbContext.KweetLikes.Remove(obj);
            await _kweetDbContext.SaveChangesAsync();

            return kweetLikeEntity;
        }

        public async Task<KweetEntity> Update(KweetEntity obj)
        {
            _kweetDbContext.Kweets.Update(obj);
            await _kweetDbContext.SaveChangesAsync();

            return await _kweetDbContext.Kweets.Where(a => a.Id == obj.Id).FirstOrDefaultAsync();
        }

        public async Task<KweetLikeEntity> GetKweetLike(int kweetId, int customerId)
        {
            return await _kweetDbContext.KweetLikes.Where(kl => kl.Kweet.Id == kweetId & kl.Customer.Id == customerId).Include(kl => kl.Kweet).Include(kl => kl.Customer).FirstOrDefaultAsync();
        }
    }
}
