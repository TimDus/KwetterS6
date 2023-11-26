using AutoMapper;
using CustomerService.API.Models.DTO;
using CustomerService.API.Models.Entity;

namespace CustomerService.API.Models.Mapper
{
    public class CustomerMapper : Profile
    {
        public CustomerMapper()
        {
            CreateMap<CustomerDTO, CustomerEntity>().ReverseMap();
        }
    }
}
