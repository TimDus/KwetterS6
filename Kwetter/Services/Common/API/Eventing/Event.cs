using MediatR;

namespace Common.Eventing
{
    public abstract class Event : IRequest
    {
        protected Event()
        {
            EventId = Guid.NewGuid();
            EventCreatedDate = DateTime.Now;
        }

        public Guid EventId { get; set; }
        public DateTime EventCreatedDate { get; set; }
    }
}