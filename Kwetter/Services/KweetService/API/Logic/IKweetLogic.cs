using KweetService.API.Models.DTO;

namespace KweetService.API.Logic
{
    public interface IKweetLogic
    {
        Task<KweetDTO> CreateKweetLogic(KweetDTO kweet);

        Task<KweetLikeDTO> LikeKweetLogic(KweetLikeDTO kweetLikeDTO);

        Task<KweetLikeDTO> UnlikeKweetLogic(KweetLikeDTO kweetLikeDTO);

        Task AddUser();
    }
}
