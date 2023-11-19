using AutoMapper;
using FollowService.API.Models.DTO;
using FollowService.API.Models.Entity;

namespace FollowService.API.Models.Mapper
{
    public class FollowMapper : Profile
    {
        public FollowMapper()
        {
            CreateMap<FollowDTO, FollowEntity>().ReverseMap();
        }
    }
}
