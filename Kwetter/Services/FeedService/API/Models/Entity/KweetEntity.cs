﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FeedService.API.Models.Entity
{
    [Table("Kweets", Schema = "dbo")]
    public class KweetEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int KweetServiceId { get; set; }

        [ForeignKey("CustomerId")]
        public CustomerEntity Customer { get; set; }

        public string Text { get; set; }

        public DateTime CreatedDate { get; set; }

        public ICollection<HashtagEntity> Hashtags { get; set; } = new List<HashtagEntity>();

        public ICollection<MentionEntity>? Mentions { get; set; } = new List<MentionEntity>();

        public ICollection<KweetLikeEntity>? Likes { get; set; }

        public KweetEntity() { }

        public KweetEntity(int kweetServiceId, string text, DateTime dateTime)
        {
            KweetServiceId = kweetServiceId;
            Text = text;
            CreatedDate = dateTime;
        }
    }
}
