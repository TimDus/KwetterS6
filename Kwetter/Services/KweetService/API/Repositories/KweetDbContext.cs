using KweetService.API.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace KweetService.API.Repositories
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
                AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
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
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CustomerEntity>()
                .HasMany(c => c.MentionedBy)
                .WithOne(mb => mb.Customer)
                .OnDelete(DeleteBehavior.NoAction);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=kweet;Username=postgres;Password=postgresSuperUserPsw");
            }
        }


        public DbSet<KweetEntity> Kweets { get; set; }

        public DbSet<KweetLikeEntity> KweetLikes { get; set; }

        public DbSet<CustomerEntity> Customers { get; set; }

        public DbSet<MentionEntity> Mentions { get; set; }

        public DbSet<HashtagEntity> Hashtags { get; set; }
    }
}
