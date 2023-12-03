namespace FeedService.API.Models.DTO
{
    public class HashtagDTO
    {
        public int Id { get; set; }

        public int KweetId { get; set; }

        public string Tag { get; set; }

        public HashtagDTO() { }

        public HashtagDTO(int kweetServiceId, int kweetId, string tag)
        {
            Id = kweetServiceId;
            KweetId = kweetId;
            Tag = tag;
        }
    }
}
