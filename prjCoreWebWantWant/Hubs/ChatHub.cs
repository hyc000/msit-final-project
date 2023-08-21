using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace prjCoreWebWantWant.Hubs
{
    public class ChatHub:Hub
    {
        public async Task SendPrivateMessage(string userId, string message)
        {
            // 將訊息傳送給特定的使用者
            await Clients.User(userId).SendAsync("ReceiveMessage", message);
        }
    }
}
