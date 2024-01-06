using ModerationService.API.Models.DTO;

namespace ModerationService.API.Logic
{
    public interface IModerationLogic
    {
        Task<List<KweetDTO>> GetPendingList();

        Task<string> CheckKweet(KweetDTO kweetDTO);
    }
}
