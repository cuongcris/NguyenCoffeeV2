using Microsoft.AspNetCore.SignalR;

namespace NguyenCoffeeWeb.Hubs
{
    public class ChatHub: Hub
    {
        public async Task SendMessage(int type, string message)
        {
            Clients.All.SendAsync("ReceiveMessage",type, message);
        }
    }
}
