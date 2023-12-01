using FeedService.API.Models.DTO;
using FeedService.API.Models.Entity;
using FeedService.API.Repositories;

namespace FeedService.API.Logic
{
    public class FeedLogic : IFeedLogic
    {
        private readonly IFeedRepository _repository;
        public FeedLogic(IFeedRepository repository) 
        {
            _repository = repository;
        }

        public async Task<List<KweetDTO>> GetFollowedKweetsFeed(int customerId)
        {
            List<KweetDTO> kweetDTOs = new();
            List<KweetEntity> kweetEntities = await _repository.GetFollowedKweetsFeed(customerId);
            foreach (KweetEntity kweet in kweetEntities)
            {
                kweetDTOs.Add(new KweetDTO());
            }
            
            return kweetDTOs;
        }

        public async Task<List<KweetDTO>> GetFollowedKweetsFeedExtension(int customerId, DateTime time)
        {
            List<KweetDTO> kweetDTOs = new();
            List<KweetEntity> kweetEntities = await _repository.GetFollowedKweetsFeedExtension(customerId, time);
            foreach (KweetEntity kweet in kweetEntities)
            {
                kweetDTOs.Add(new KweetDTO());
            }

            return kweetDTOs;
        }

        public async Task<List<KweetDTO>> GetRandomKweetsFeed()
        {
            List<KweetDTO> kweetDTOs = new();
            List<KweetEntity> kweetEntities = await _repository.GetRandomKweetsFeed();
            foreach (KweetEntity kweet in kweetEntities)
            {
                kweetDTOs.Add(new KweetDTO());
            }

            return kweetDTOs;
        }

        public async Task<List<KweetDTO>> GetRandomKweetsFeedExtension(DateTime time)
        {
            List<KweetDTO> kweetDTOs = new();
            List<KweetEntity> kweetEntities = await _repository.GetRandomKweetsFeedExtension(time);
            foreach (KweetEntity kweet in kweetEntities)
            {
                kweetDTOs.Add(new KweetDTO());
            }

            return kweetDTOs;
        }
    }
}
