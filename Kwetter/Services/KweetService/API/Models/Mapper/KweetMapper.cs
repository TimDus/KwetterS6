using AutoMapper;
using KweetService.API.Models.DTO;
using KweetService.API.Models.Entity;

namespace KweetService.API.Models.Mapper
{
    public class KweetMapper : Profile
    {
        public KweetMapper()
        {
            CreateMap<KweetDTO, KweetEntity>().ReverseMap();
        }
    }
}