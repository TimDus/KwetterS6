using Common.Interfaces;
using CustomerService.API.Models.Entity;

namespace CustomerService.API.Repositories
{
    public interface ICustomerRepository : IGenericRepository<CustomerEntity>
    {
    }
}
