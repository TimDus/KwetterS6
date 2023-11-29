﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KweetService.API.Models.Entity
{
    [Table("Customer", Schema = "dbo")]
    public class CustomerEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public string DisplayName { get; set; }

        public string CustomerName { get; set; }

        public string? ProfilePicture { get; set; }

        public ICollection<KweetEntity>? Kweets { get; set; }

        public ICollection<KweetLikeEntity>? LikedKweets { get; set; }

        public ICollection<MentionEntity>? MentionedBy { get; set; }

        public CustomerEntity() { }

        public CustomerEntity(int customerId, string displayName, string customerName)
        {
            CustomerId = customerId;
            DisplayName = displayName;
            CustomerName = customerName;
        }
    }
}