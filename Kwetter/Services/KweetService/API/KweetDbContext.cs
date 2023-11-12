﻿using KweetService.API.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace KweetService.API
{
    public class KweetDbContext : DbContext
    {
        public KweetDbContext(DbContextOptions<KweetDbContext> dbContextOptions) : base(dbContextOptions)
        {
            try
            {
                RelationalDatabaseCreator databaseCreator = (RelationalDatabaseCreator)Database.GetService<IDatabaseCreator>();
                if (databaseCreator != null)
                {
                    if (!databaseCreator.CanConnect()) databaseCreator.Create();
                    if (!databaseCreator.HasTables()) databaseCreator.CreateTables();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public DbSet<KweetEntity> Kweets { get; set; }

        public DbSet<KweetLikeEntity> KweetLikes { get; set; }

        public DbSet<CustomerEntity> Customers { get; set; }

        public DbSet<MentionEntity> Mentions { get; set; }

        public DbSet<HashtagEntity> Hashtags { get; set; }
    }
}
