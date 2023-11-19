﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FollowService.API.Models.Entity
{
    public class CustomerEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int AccountId { get; set; }

        public string DisplayName { get; set; }

        public string CustomerName { get; set; }

        public string CustomerProfilePicture { get; set; }

        public ICollection<FollowEntity> Followers { get; set; }

        public ICollection<FollowEntity> Following { get; set; }
    }
}
