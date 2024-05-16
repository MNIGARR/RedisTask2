namespace RedisTask2.Services
{
    public interface IMessageListener
    {
        public Task SubscribeChannel(string channel);
    }
}
