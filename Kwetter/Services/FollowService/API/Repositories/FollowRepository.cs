﻿using FollowService.API.Models.Entity;
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

        public async Task<CustomerEntity> AddCustomer(CustomerEntity obj)
        {
            await _followDbContext.Customers.AddAsync(obj);
            await _followDbContext.SaveChangesAsync();

            return _followDbContext.Customers.Where(a => a.Id == obj.Id).FirstOrDefault();
        }

        public async Task<FollowEntity> Create(FollowEntity obj)
        {
            await _followDbContext.Follows.AddAsync(obj);
            await _followDbContext.SaveChangesAsync();

            return _followDbContext.Follows.Where(a => a.FollowerId == obj.FollowerId).FirstOrDefault();
        }

        public async Task Delete(FollowEntity obj)
        {
            _followDbContext.Follows.Remove(obj);
            await _followDbContext.SaveChangesAsync();

            return;
        }

        public Task<FollowEntity> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<FollowEntity>> GetFollowers(int accountId)
        {
            return await _followDbContext.Follows.Where(a => a.FollowingId == accountId).ToListAsync();
        }

        public async Task<List<FollowEntity>> GetFollowing(int accountId)
        {
            return await _followDbContext.Follows.Where(a => a.FollowerId == accountId).ToListAsync();
        }

        public Task<FollowEntity> Update(FollowEntity obj)
        {
            throw new NotImplementedException();
        }
    }
}
