﻿namespace KweetService.API.Models.DTO
{
    public class MentionDTO
    {
        public int Id { get; set; }

        public int MentionedCustomerId { get; set; }

        public int KweetId { get; set; }

        public MentionDTO(int id, int mentionedCustomerId, int kweetId)
        {
            Id = id;
            MentionedCustomerId = mentionedCustomerId;
            KweetId = kweetId;
        }

        public MentionDTO() { }
    }
}
