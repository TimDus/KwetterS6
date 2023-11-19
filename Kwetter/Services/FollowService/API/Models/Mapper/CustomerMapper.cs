using AutoMapper;
using FollowService.API.Models.DTO;
using FollowService.API.Models.Entity;

namespace FollowService.API.Models.Mapper
{
    public class CustomerMapper : Profile
    {
        public CustomerMapper()
        {
            CreateMap<CustomerDTO, CustomerEntity>().ReverseMap();
        }
    }
}
