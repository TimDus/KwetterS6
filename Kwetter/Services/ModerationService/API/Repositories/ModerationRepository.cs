using Microsoft.EntityFrameworkCore;
using ModerationService.API.Models.Entity;

namespace ModerationService.API.Repositories
{
    public class ModerationRepository : IModerationRepository
    {
        private readonly ModerationDbContext _moderationDbContext;

        public ModerationRepository(ModerationDbContext moderationDbContext)
        {
            _moderationDbContext = moderationDbContext;
        }

        public async Task<CustomerEntity> CreateCustomer(CustomerEntity obj)
        {
            await _moderationDbContext.Customers.AddAsync(obj);
            await _moderationDbContext.SaveChangesAsync();

            return await _moderationDbContext.Customers.Where(a => a.Id == obj.Id).FirstOrDefaultAsync();
        }

        public async Task<KweetEntity> Create(KweetEntity obj)
        {
            await _moderationDbContext.Kweets.AddAsync(obj);
            await _moderationDbContext.SaveChangesAsync();

            return await _moderationDbContext.Kweets.Where(k => k.Id == obj.Id).FirstOrDefaultAsync();
        }

        public Task<KweetEntity> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<KweetEntity> GetById(int id)
        {
            return await _moderationDbContext.Kweets.Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<CustomerEntity> GetCustomer(int id)
        {
            return await _moderationDbContext.Customers.Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public Task<KweetEntity> Update(KweetEntity obj)
        {
            throw new NotImplementedException();
        }
    }
}
