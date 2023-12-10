using ModerationService.API.Models.DTO;

namespace ModerationService.API.Logic
{
    public interface IModerationLogic
    {
        Task<List<KweetDTO>> GetList();

        Task<KweetDTO> CheckKweet(KweetDTO kweetDTO);
    }
}
