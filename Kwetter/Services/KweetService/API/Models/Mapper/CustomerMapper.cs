using AutoMapper;
using KweetService.API.Models.DTO;
using KweetService.API.Models.Entity;

namespace KweetService.API.Models.Mapper
{
    public class CustomerMapper : Profile
    {
        public CustomerMapper() 
        {
            CreateMap<CustomerDTO, CustomerEntity>().ReverseMap();
        }
    }
}
