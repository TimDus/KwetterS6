﻿using KweetService.API.Models.DTO;

namespace KweetService.API.Logic
{
    public interface IKweetLogic
    {
        Task<KweetCreatedDTO> CreateKweetLogic(KweetCreatedDTO kweetDTO);

        Task<KweetLikeDTO> LikeKweetLogic(KweetLikeDTO kweetLikeDTO);

        Task<KweetLikeDTO> UnlikeKweetLogic(KweetLikeDTO kweetLikeDTO);

        Task<List<KweetCreatedDTO>> GetRandomKweetsFeed();
    }
}
