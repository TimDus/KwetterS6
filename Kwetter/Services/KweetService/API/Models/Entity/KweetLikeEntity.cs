﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KweetService.API.Models.Entity
{
    public class KweetLikeEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [ForeignKey("kweet")]
        [Column("kweetid")]
        public int KwetId { get; set; }

        [ForeignKey("customer")]
        [Column("customerid")]
        public int CustomerId { get; set; }

        [Column("likeddatetime")]
        public DateTime LikedDateTime { get; set; }
    }
}
