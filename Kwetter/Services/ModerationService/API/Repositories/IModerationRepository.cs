using Common.Interfaces;
using ModerationService.API.Models.Entity;

namespace ModerationService.API.Repositories
{
    public interface IModerationRepository : IGenericRepository<KweetEntity>
    {
        Task<CustomerEntity> GetCustomer(int id);

        Task<CustomerEntity> CreateCustomer(CustomerEntity obj);

        Task<List<KweetEntity>> GetPendingList();
    }
}
