﻿using FollowService.API.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace FollowService.API.Repositories
{
    public class FollowDBContext : DbContext
    {
        public FollowDBContext(DbContextOptions<FollowDBContext> dbContextOptions) : base(dbContextOptions)
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerEntity>()
                .HasMany(c => c.Followers)
                .WithOne(f => f.Following)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<CustomerEntity>()
                .HasMany(c => c.Following)
                .WithOne(f => f.Follower)
                .OnDelete(DeleteBehavior.ClientCascade);
        }

        public DbSet<CustomerEntity> Customers { get; set; }

        public DbSet<FollowEntity> Follows { get; set; }
    }
}
