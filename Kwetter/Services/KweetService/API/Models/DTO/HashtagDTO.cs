namespace KweetService.API.Models.DTO
{
    public class HashtagDTO
    {
        public int Id { get; set; }

        public KweetDTO Kweet { get; set; }

        public string Tag { get; set; }
    }
}
