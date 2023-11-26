using AutoMapper;
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
        private readonly IMapper _mapper;

        public CustomerLogic(IMediator mediator, ICustomerRepository repository, IMapper mapper)
        {
            _mediator = mediator;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CustomerDTO> CreateCustomerLogic(CustomerDTO customerDTO)
        {
            CustomerEntity customerEntity = _mapper.Map<CustomerEntity>(customerDTO);

            customerEntity = await _repository.Create(customerEntity);

            var customer = new CustomerCreatedEvent
            {
                CustomerId = customerEntity.Id,
                CustomerName = customerEntity.CustomerName,
                DisplayName = customerEntity.DisplayName,
                ProfilePicture = customerEntity.ProfilePicture
            };

            await _mediator.Send(customer);

            return _mapper.Map<CustomerDTO>(customerEntity);
        }
    }
}
