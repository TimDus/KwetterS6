﻿namespace KweetService.API.Models.DTO
{
    public class HashtagDTO
    {
        public int Id { get; set; }

        public int KweetId { get; set; }

        public string Tag { get; set; }

        public HashtagDTO() { }

        public HashtagDTO(int id, int kweetId, string tag)
        {
            Id = id;
            KweetId = kweetId;
            Tag = tag;
        }
    }
}
