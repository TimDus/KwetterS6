using FollowService.API.Models.Entity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;

namespace FollowService.API.Repositories
{
    public class FollowRepository : IFollowRepository
    {
        private readonly FollowDBContext _followDbContext;

        public FollowRepository(FollowDBContext followDbContext)
        {
            _followDbContext = followDbContext;
        }

        public async Task<CustomerEntity> CreateCustomer(CustomerEntity obj)
        {
            await _followDbContext.Customers.AddAsync(obj);
            await _followDbContext.SaveChangesAsync();

            return await _followDbContext.Customers.Where(c => c.Id == obj.Id).FirstOrDefaultAsync();
        }

        public async Task<FollowEntity> Create(FollowEntity obj)
        {
            await _followDbContext.Follows.AddAsync(obj);
            await _followDbContext.SaveChangesAsync();

            return await _followDbContext.Follows.Where(f => f.Id == obj.Id).FirstOrDefaultAsync();
        }

        public async Task<FollowEntity> Delete(int id)
        {
            FollowEntity follow =  await _followDbContext.Follows.Where(f => f.Id == id).Include(f => f.Follower).Include(f => f.Following).FirstOrDefaultAsync();
            _followDbContext.Follows.Remove(follow);
            await _followDbContext.SaveChangesAsync();

            return follow;
        }

        public Task<FollowEntity> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<FollowEntity>> GetFollowers(int accountId)
        {
            return await _followDbContext.Follows.Where(f => f.Following.Id == accountId).ToListAsync();
        }

        public async Task<List<FollowEntity>> GetFollowing(int accountId)
        {
            return await _followDbContext.Follows.Where(f => f.Follower.Id == accountId).ToListAsync();
        }

        public Task<FollowEntity> Update(FollowEntity obj)
        {
            throw new NotImplementedException();
        }

        public async Task<CustomerEntity> GetCustomer(int id)
        {
            return await _followDbContext.Customers.Where(c => c.Id == id).FirstOrDefaultAsync();
        }
    }
}
