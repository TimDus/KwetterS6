using AutoMapper;
using KweetService.API.Models.DTO;
using KweetService.API.Models.Entity;

namespace KweetService.API.Models.Mapper
{
    public class HashtagMapper : Profile
    {
        public HashtagMapper()
        {
            CreateMap<HashtagDTO, HashtagEntity>().ReverseMap();
        }
    }
}
