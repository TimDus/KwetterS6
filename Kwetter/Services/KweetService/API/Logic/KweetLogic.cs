using KweetService.API.Eventing.EventPublisher.KweetCreated;
using KweetService.API.Eventing.EventPublisher.KweetLiked;
using KweetService.API.Eventing.EventPublisher.KweetUnliked;
using KweetService.API.Models.DTO;
using KweetService.API.Models.Entity;
using KweetService.API.Repositories;
using MediatR;

namespace KweetService.API.Logic
{
    public class KweetLogic : IKweetLogic
    {
        private readonly IMediator _mediator;
        private readonly IKweetRepository _repository;

        public KweetLogic(IMediator mediator, IKweetRepository repository)
        {
            _mediator = mediator;
            _repository = repository;
        }

        public async Task<KweetCreatedDTO> CreateKweetLogic(KweetCreatedDTO kweetDTO)
        {
            kweetDTO.CreatedDate = DateTime.Now;
            KweetEntity kweetEntity = new(kweetDTO.Text, kweetDTO.CreatedDate);

            if(kweetDTO.Hashtags != null)
            {
                foreach (HashtagDTO hashtag in kweetDTO.Hashtags)
                {
                    kweetEntity.Hashtags.Add(new HashtagEntity(hashtag.Tag));
                };
            }

            if(kweetDTO.Mentions != null)
            {
                foreach (MentionDTO mention in kweetDTO.Mentions)
                {
                    kweetEntity.Mentions.Add(new MentionEntity() { Customer = await _repository.GetCustomer(mention.MentionedCustomerId) });
                };
            }


            kweetEntity.Customer = await _repository.GetCustomer(kweetDTO.CustomerId);

            kweetEntity = await _repository.Create(kweetEntity);

            var kweetEvent = new KweetCreatedEvent
            {
                KweetId = kweetEntity.Id,
                CustomerId = kweetEntity.Customer.Id,
                Text = kweetEntity.Text,
                KweetCreatedDate = kweetEntity.CreatedDate,
            };

            foreach(MentionEntity mention in kweetEntity.Mentions)
            {
                kweetEvent.Mentions.Add(new MentionDTO(mention.Id, mention.Customer.Id, mention.Kweet.Id));
            }

            foreach(HashtagEntity hashtag in kweetEntity.Hashtags)
            {
                kweetEvent.Hashtags.Add(new HashtagDTO(hashtag.Id, hashtag.Kweet.Id, hashtag.Tag));
            }

            await _mediator.Send(kweetEvent);

            kweetDTO.Id = kweetEntity.Id;

            return kweetDTO;
        }

        public async Task<KweetLikeDTO> LikeKweetLogic(KweetLikeDTO kweetLikeDTO)
        {
            KweetLikeEntity kweetLikeEntity = new(
                kweetLikeDTO.LikedDateTime,
                await _repository.GetCustomer(kweetLikeDTO.CustomerId),
                await _repository.GetById(kweetLikeDTO.KweetId)
                );


            KweetLikeEntity like = await _repository.LikeKweet(kweetLikeEntity);

            var kweet = new KweetLikedEvent
            {
                LikeId = like.Id,
                CustomerId = like.Customer.Id,
                KweetId = like.Kweet.Id,
                LikedDateTime = like.LikedDateTime
            };

            await _mediator.Send(kweet);

            kweetLikeDTO.Id = kweetLikeEntity.Id;

            return kweetLikeDTO;
        }

        public async Task<KweetLikeDTO> UnlikeKweetLogic(KweetLikeDTO kweetLikeDTO)
        {
            KweetLikeEntity kweetLikeEntity = await _repository.GetKweetLike(kweetLikeDTO.KweetId, kweetLikeDTO.CustomerId);

            KweetLikeEntity like = await _repository.UnlikeKweet(kweetLikeEntity);

            var kweet = new KweetUnlikedEvent
            {
                LikeId = like.Id,
                CustomerId = like.Customer.Id,
                KweetId = like.Kweet.Id,
            };

            kweetLikeDTO.Id = kweetLikeEntity.Id;

            await _mediator.Send(kweet);

            return kweetLikeDTO;
        }
    }
}
