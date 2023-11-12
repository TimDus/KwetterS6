using KweetService.API.Models.DTO;

namespace KweetService.API.Logic
{
    public interface IKweetLogic
    {
        Task<KweetDTO> CreateKweetLogic(KweetDTO kweet);

        Task<KweetDTO> LikeKweetLogic(KweetDTO kweet);

        Task<KweetDTO> UnlikeKweetLogic(KweetDTO kweet);
    }
}
