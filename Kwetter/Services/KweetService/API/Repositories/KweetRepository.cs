using KweetService.API.Models.Entity;
using Microsoft.AspNetCore.Mvc;

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

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<KweetEntity> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task LikeKweet(int id)
        {
            throw new NotImplementedException();
        }

        public Task UnlikeKweet(int id)
        {
            throw new NotImplementedException();
            //return Task.CompletedTask;
        }

        public Task Update(KweetEntity obj)
        {
            throw new NotImplementedException();
        }
    }
}
