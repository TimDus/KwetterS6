using KweetService.API.Models.DTO;

namespace KweetService.API.Logic
{
    public interface IKweetLogic
    {
        Task<KweetCreateDTO> CreateKweetLogic(KweetCreateDTO kweet);

        Task<KweetLikeDTO> LikeKweetLogic(KweetLikeDTO kweetLikeDTO);

        Task<KweetLikeDTO> UnlikeKweetLogic(KweetLikeDTO kweetLikeDTO);
    }
}
