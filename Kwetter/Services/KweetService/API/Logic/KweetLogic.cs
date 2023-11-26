using AutoMapper;
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
        private readonly IMapper _mapper;

        public KweetLogic(IMediator mediator, IKweetRepository repository, IMapper mapper)
        {
            _mediator = mediator;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<KweetDTO> CreateKweetLogic(KweetDTO kweetDTO)
        {
            KweetEntity kweetEntity = _mapper.Map<KweetEntity>(kweetDTO);

            kweetEntity = await _repository.Create(kweetEntity);

            var kweet = new KweetCreatedEvent
            {
                KweetId = kweetEntity.Id,
                CustomerId = kweetEntity.Customer.Id,
                Text = kweetEntity.Text,
                KweetCreatedDate = kweetEntity.CreatedDate
            };

            await _mediator.Send(kweet);

            return _mapper.Map<KweetDTO>(kweetEntity);
        }

        public async Task<KweetLikeDTO> LikeKweetLogic(KweetLikeDTO kweetLikeDTO)
        {
            KweetLikeEntity kweetLikeEntity = _mapper.Map<KweetLikeEntity>(kweetLikeDTO);

            KweetLikeEntity like = await _repository.LikeKweet(kweetLikeEntity);

            var kweet = new KweetLikedEvent
            {
                LikeId = like.Id,
                CustomerId = like.Customer.Id,
                KweetId = like.Kweet.Id,
                LikedDateTime = like.LikedDateTime
            };

            await _mediator.Send(kweet);

            return kweetLikeDTO;
        }

        public async Task<KweetLikeDTO> UnlikeKweetLogic(KweetLikeDTO kweetLikeDTO)
        {
            KweetLikeEntity kweetLikeEntity = _mapper.Map<KweetLikeEntity>(kweetLikeDTO);

            KweetLikeEntity like = await _repository.UnlikeKweet(kweetLikeEntity);

            var kweet = new KweetUnlikedEvent
            {
                LikeId = like.Id,
                CustomerId = like.Customer.Id,
                KweetId = like.Kweet.Id,
                LikedDateTime = like.LikedDateTime
            };

            await _mediator.Send(kweet);

            return kweetLikeDTO;
        }

        public async Task AddUser()
        {
            await _repository.AddCustomer(new CustomerEntity());
        }
    }
}
