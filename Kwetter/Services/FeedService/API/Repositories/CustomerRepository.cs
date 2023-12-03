using FeedService.API.Models.Entity;
using FeedService.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FeedService.API.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly FeedDbContext _feedDbContext;

        public CustomerRepository(FeedDbContext dbContext)
        {
            _feedDbContext = dbContext;
        }

        public async Task<CustomerEntity> Create(CustomerEntity obj)
        {
            await _feedDbContext.Customers.AddAsync(obj);
            await _feedDbContext.SaveChangesAsync();

            return await _feedDbContext.Customers.Where(a => a.Id == obj.Id).FirstOrDefaultAsync();
        }

        public Task<CustomerEntity> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<CustomerEntity> GetById(int id)
        {
            return await _feedDbContext.Customers.Where(a => a.CustomerId == id).FirstOrDefaultAsync();
        }

        public Task<CustomerEntity> Update(CustomerEntity obj)
        {
            throw new NotImplementedException();
        }
    }
}
