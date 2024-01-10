using CustomerService.API.Models.Auth;
using CustomerService.API.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.API.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerDBContext _customerDBContext;

        public CustomerRepository(CustomerDBContext customerDBContext)
        {
            _customerDBContext = customerDBContext;
        }

        public async Task<CustomerEntity> Create(CustomerEntity obj)
        {
            await _customerDBContext.Customers.AddAsync(obj);
            await _customerDBContext.SaveChangesAsync();

            return await _customerDBContext.Customers.Where(a => a.Id == obj.Id).FirstOrDefaultAsync();
        }

        public Task<CustomerEntity> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<CustomerEntity> GetById(int id)
        {
            return await _customerDBContext.Customers.Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<CustomerEntity> GetByName(string customerName)
        {
            return await _customerDBContext.Customers.Where(c => c.CustomerName == customerName).FirstOrDefaultAsync();
        }

        public async Task<CustomerEntity> Update(CustomerEntity obj)
        {
            _customerDBContext.Customers.Update(obj);
            await _customerDBContext.SaveChangesAsync();

            return await _customerDBContext.Customers.Where(a => a.Id == obj.Id).FirstOrDefaultAsync();
        }
    }
}
