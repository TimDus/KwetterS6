using FeedService.API.Models.Entity;

namespace FeedService.API.Repositories
{
    public class FeedRepository : IFeedRepository
    {
        private readonly FeedDbContext _feedDbContext;

        public FeedRepository(FeedDbContext dbContext) 
        {
            _feedDbContext = dbContext;
        }

        public async Task<CustomerEntity> CreateCustomer(CustomerEntity obj)
        {
            await _feedDbContext.Customers.AddAsync(obj);
            await _feedDbContext.SaveChangesAsync();

            return _feedDbContext.Customers.Where(a => a.Id == obj.Id).FirstOrDefault();
        }

        public async Task<KweetEntity> CreateKweet(KweetEntity obj)
        {
            await _feedDbContext.Kweets.AddAsync(obj);
            await _feedDbContext.SaveChangesAsync();

            return _feedDbContext.Kweets.Where(a => a.Id == obj.Id).FirstOrDefault();
        }

        public async Task<FollowEntity> FollowCustomer(FollowEntity obj)
        {
            throw new NotImplementedException();
        }

        public async Task<CustomerEntity> GetCustomer(int id)
        {
            return await Task.FromResult<CustomerEntity>(_feedDbContext.Customers.Where(a => a.Id == id).FirstOrDefault());
        }

        public async Task<KweetEntity> GetKweet(int id)
        {
            return await Task.FromResult<KweetEntity>(_feedDbContext.Kweets.Where(a => a.Id == id).FirstOrDefault());
        }

        public async Task<KweetLikeEntity> LikeKweet(KweetLikeEntity obj)
        {
            throw new NotImplementedException();
        }

        public async Task<FollowEntity> UnfollowCustomer(FollowEntity obj)
        {
            throw new NotImplementedException();
        }

        public async Task<KweetLikeEntity> UnlikeKweet(KweetLikeEntity obj)
        {
            throw new NotImplementedException();
        }

        public async Task<KweetEntity> UpdateKweet(KweetEntity obj)
        {
            throw new NotImplementedException();
        }
    }
}
