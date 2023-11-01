using MediatR;

namespace KweetService.API.Temp
{
    public class CreateKweetCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Kweet { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
