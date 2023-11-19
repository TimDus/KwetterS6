using AutoMapper;
using KweetService.API.Models.DTO;
using KweetService.API.Models.Entity;

namespace KweetService.API.Models.Mapper
{
    public class MentionMapper : Profile
    {
        public MentionMapper()
        {
            CreateMap<MentionDTO, MentionEntity>().ReverseMap();
        }
    }
}
