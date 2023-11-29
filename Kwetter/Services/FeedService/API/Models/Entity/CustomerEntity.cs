namespace FeedService.API.Models.Entity
{
    public class CustomerEntity
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public string DisplayName { get; set; }

        public string CustomerName { get; set; }

        public string? ProfilePicture { get; set; }

        public ICollection<KweetEntity>? Kweets { get; set; }

        public ICollection<KweetLikeEntity>? LikedKweets { get; set; }

        public ICollection<FollowEntity>? Following { get; set; }

        public ICollection<FollowEntity>? Followers { get; set; }

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
