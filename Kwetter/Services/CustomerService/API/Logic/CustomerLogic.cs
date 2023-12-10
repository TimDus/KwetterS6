using CustomerService.API.Eventing.EventPublisher.CustomerCreated;
using CustomerService.API.Models.DTO;
using CustomerService.API.Models.Entity;
using CustomerService.API.Repositories;
using MediatR;

namespace CustomerService.API.Logic
{
    public class CustomerLogic : ICustomerLogic
    {
        private readonly IMediator _mediator;
        private readonly ICustomerRepository _repository;

        public CustomerLogic(IMediator mediator, ICustomerRepository repository)
        {
            _mediator = mediator;
            _repository = repository;
        }

        public async Task<CustomerCreateDTO> CreateCustomerLogic(CustomerCreateDTO customerDTO)
        {
            CustomerEntity customerEntity = new(customerDTO.AccountId, customerDTO.DisplayName, customerDTO.CustomerName);

            customerEntity = await _repository.Create(customerEntity);

            var customer = new CustomerCreatedEvent
            {
                CustomerId = customerEntity.Id,
                CustomerName = customerEntity.CustomerName,
                DisplayName = customerEntity.DisplayName,
            };

            await _mediator.Send(customer);

            customerDTO.Id = customerEntity.Id;

            return customerDTO;
        }
    }
}
