using Microsoft.AspNetCore.SignalR;


namespace RedisTask2.Hubs
{
    public class ChatHub: Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.Others.SendAsync("Connect");
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Clients.Others.SendAsync("Disconnect");
        }
    }
}
