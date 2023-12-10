using FeedService.API.Models.DTO;
using FeedService.API.Models.Entity;
using FeedService.API.Repositories.Interfaces;
using Microsoft.IdentityModel.Tokens;

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
            List<FollowEntity> followEntities = await _repository.GetFollowedKweetsFeed(customerId);
            List<CustomerEntity> followedCustomers = new();
            foreach (FollowEntity follow in followEntities)
            {
                followedCustomers.Add(follow.Following);
            }

            List<KweetDTO> kweetDTOs = MakeFeed(followedCustomers);

            return kweetDTOs;
        }

        public async Task<List<KweetDTO>> GetFollowedKweetsFeedExtension(int customerId, DateTime time)
        {
            List<FollowEntity> followEntities = await _repository.GetFollowedKweetsFeedExtension(customerId, time);
            List<CustomerEntity> followedCustomers = new();
            foreach (FollowEntity follow in followEntities)
            {
                followedCustomers.Add(follow.Following);
            }

            List<KweetDTO> kweetDTOs = MakeFeed(followedCustomers);

            return kweetDTOs;
        }

        public async Task<List<KweetDTO>> GetRandomKweetsFeed(int id)
        {
            List<KweetDTO> kweetDTOs = MakeFeed(await _repository.GetRandomKweetsFeed(id));

            return kweetDTOs;
        }

        public async Task<List<KweetDTO>> GetRandomKweetsFeedExtension(int id, DateTime time)
        {
            List<KweetDTO> kweetDTOs = MakeFeed(await _repository.GetRandomKweetsFeedExtension(id, time));

            return kweetDTOs;
        }

        private List<KweetDTO> MakeFeed(List<CustomerEntity> customerEntities)
        {
            List<KweetDTO> kweetDTOs = new();
            foreach (CustomerEntity customer in customerEntities)
            {
                foreach (KweetEntity kweet in customer.Kweets)
                {
                    KweetDTO ToBeAdded = new()
                    {
                        ProfilePicture = customer.ProfilePicture,
                        CustomerName = customer.CustomerName,
                        DisplayName = customer.DisplayName,
                        CustomerId = customer.CustomerId,
                        KweetServiceId = kweet.KweetServiceId,
                        Text = kweet.Text,
                        CreatedDate = kweet.CreatedDate,
                    };
                    if (!kweet.Likes.IsNullOrEmpty())
                    {
                        ToBeAdded.Liked = true;
                    }
                    else
                    {
                        ToBeAdded.Liked = false;
                    }
                    foreach (HashtagEntity hashtag in kweet.Hashtags)
                    {
                        ToBeAdded.Hashtags.Add(new HashtagDTO(hashtag.KweetServiceId, hashtag.Kweet.Id, hashtag.Tag));
                    }
                    foreach (MentionEntity mention in kweet.Mentions)
                    {
                        ToBeAdded.Mentions.Add(new MentionDTO(mention.KweetServiceId, mention.Customer.Id, mention.Kweet.Id));
                    }
                    kweetDTOs.Add(ToBeAdded);
                }
            }

            kweetDTOs.Sort((x, y) => DateTime.Compare(y.CreatedDate, x.CreatedDate));
            return kweetDTOs;
        }
    }
}
