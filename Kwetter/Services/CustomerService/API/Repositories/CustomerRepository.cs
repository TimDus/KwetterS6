using CustomerService.API.Models.Entity;

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

            return _customerDBContext.Customers.Where(a => a.Id == obj.Id).FirstOrDefault();
        }

        public async Task Delete(CustomerEntity obj)
        {
            throw new NotImplementedException();
        }

        public async Task<CustomerEntity> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<CustomerEntity> Update(CustomerEntity obj)
        {
            throw new NotImplementedException();
        }
    }
}
