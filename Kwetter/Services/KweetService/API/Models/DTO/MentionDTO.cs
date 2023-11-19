using KweetService.API.Models.Entity;

namespace KweetService.API.Models.DTO
{
    public class MentionDTO
    {
        public int Id { get; set; }

        public CustomerDTO Customer { get; set; }

        public KweetDTO Kweet { get; set; }
    }
}
