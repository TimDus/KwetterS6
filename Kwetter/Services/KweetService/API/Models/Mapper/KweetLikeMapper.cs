using AutoMapper;
using KweetService.API.Models.DTO;
using KweetService.API.Models.Entity;

namespace KweetService.API.Models.Mapper
{
    public class KweetLikeMapper : Profile
    {
        public KweetLikeMapper()
        {
            CreateMap<KweetLikeDTO, KweetLikeEntity>().ReverseMap();
        }
    }
}
