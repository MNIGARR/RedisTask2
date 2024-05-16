using Microsoft.AspNetCore.SignalR;
using RedisTask2.Hubs;
using StackExchange.Redis;

namespace RedisTask2.Services
{
    public class MessageListener: IMessageListener
    {
        private readonly IConnectionMultiplexer _redis;
        private string? _currentChannel;
        private readonly IHubContext<ChatHub> _hubContext;
        private static List<string> _messages = new List<string>();
        public MessageListener(IConnectionMultiplexer redis, IHubContext<ChatHub> hubContext)
        {
            _redis = redis;
            _hubContext = hubContext;
        }

        public async Task SubscribeChannel(string channel)
        {
            var subscriber = _redis.GetSubscriber();
            if (subscriber != null && !string.IsNullOrEmpty(channel))
            {

                if (_currentChannel != null && _currentChannel != channel)
                {
                    subscriber.Unsubscribe(_currentChannel);
                }

                _currentChannel = channel;


                await subscriber.SubscribeAsync(_currentChannel, (channel, message) =>
                {
                    string receivedMessage = message.ToString();
                    var time = DateTime.Now.ToLongTimeString();
                    var checkmessage = $"{receivedMessage}_{time}";
                    if (_messages.Contains(checkmessage))
                    {
                        return;
                    }
                    _hubContext.Clients.All.SendAsync("new message", new { channel = _currentChannel, message = receivedMessage, time = time });
                    _messages.Add(checkmessage);
                });
            }
        }
    }
}
