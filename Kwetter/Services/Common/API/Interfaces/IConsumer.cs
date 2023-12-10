namespace Common.Interfaces
{
    public interface IConsumer<T> where T : class
    {
        public Task<T> ReadMessages();
    }
}
