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

        public async Task<CustomerEntity> AddCustomer(CustomerEntity obj)
        {
            await _kweetDbContext.Customers.AddAsync(obj);
            await _kweetDbContext.SaveChangesAsync();

            return _kweetDbContext.Customers.Where(a => a.Id == obj.Id).FirstOrDefault();
        }

        public async Task<KweetEntity> Create(KweetEntity obj)
        {
            await _kweetDbContext.Kweets.AddAsync(obj);
            await _kweetDbContext.SaveChangesAsync();

            return _kweetDbContext.Kweets.Where(a => a.Id == obj.Id).FirstOrDefault();
        }

        public async Task Delete(KweetEntity obj)
        {
            _kweetDbContext.Kweets.Remove(obj);
            await _kweetDbContext.SaveChangesAsync();

            return;
        }

        public Task<KweetEntity> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<KweetLikeEntity> LikeKweet(KweetLikeEntity obj)
        {
            await _kweetDbContext.KweetLikes.AddAsync(obj);
            await _kweetDbContext.SaveChangesAsync();

            try
            {
                return _kweetDbContext.KweetLikes.Where(a => a.Id == obj.Id).FirstOrDefault();
            }
            catch
            {
                return new KweetLikeEntity();
            }
        }

        public async Task<KweetLikeEntity> UnlikeKweet(KweetLikeEntity obj)
        {
            _kweetDbContext.KweetLikes.Remove(obj);
            await _kweetDbContext.SaveChangesAsync();

            return _kweetDbContext.KweetLikes.Where(a => a.Id == obj.Id).FirstOrDefault();
        }

        public async Task<KweetEntity> Update(KweetEntity obj)
        {
            _kweetDbContext.Kweets.Update(obj);
            await _kweetDbContext.SaveChangesAsync();

            return _kweetDbContext.Kweets.Where(a => a.Id == obj.Id).FirstOrDefault();
        }
    }
}
