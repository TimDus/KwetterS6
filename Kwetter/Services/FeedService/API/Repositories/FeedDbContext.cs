using FeedService.API.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace FeedService.API.Repositories
{
    public class FeedDbContext : DbContext
    {
        public FeedDbContext(DbContextOptions<FeedDbContext> dbContextOptions) : base(dbContextOptions)
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
                .HasMany(c => c.LikedKweets)
                .WithOne(kl => kl.Customer)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<CustomerEntity>()
                .HasMany(c => c.MentionedBy)
                .WithOne(mb => mb.Customer)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<CustomerEntity>()
                .HasMany(c => c.Followers)
                .WithOne(f => f.Following)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<CustomerEntity>()
                .HasMany(c => c.Following)
                .WithOne(f => f.Follower)
                .OnDelete(DeleteBehavior.ClientCascade);
        }


        public DbSet<KweetEntity> Kweets { get; set; }

        public DbSet<KweetLikeEntity> KweetLikes { get; set; }

        public DbSet<CustomerEntity> Customers { get; set; }

        public DbSet<MentionEntity> Mentions { get; set; }

        public DbSet<HashtagEntity> Hashtags { get; set; }

        public DbSet<FollowEntity> Follows { get; set; }
    }
}
