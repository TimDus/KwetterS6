using KweetService.API.Models.Entity;

namespace KweetService.API.Models.DTO
{
    public class KweetDTO
    {
        public long Id { get; set; }
        public CustomerDTO Customer { get; set; }
        public string Text { get; set; }
        public List<string>? Hashtag { get; set; }
        public List<string>? Mentions { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
