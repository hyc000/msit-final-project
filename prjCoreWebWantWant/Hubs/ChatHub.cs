using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using prjCoreWebWantWant.Models;

namespace prjCoreWebWantWant.Hubs
{
    public class ChatHub:Hub
    {

        private readonly NewIspanProjectContext _db;
        public ChatHub(NewIspanProjectContext dbContext)
        {
            _db = dbContext;
        }

        public async Task SendPrivateMessage(int senderId, int receiverId, string message)
        {
            //先存入資料庫
            var chatMessage = new ChatMessage
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Message = message,
                Created = DateTime.Now
            };
            _db.ChatMessages.Add(chatMessage);
            await _db.SaveChangesAsync();

            // 將訊息傳送給特定的使用者
            //await Clients.User(receiverId.ToString()).SendAsync("ReceiveMessage", senderId, message);
            await Clients.User(Context.UserIdentifier).SendAsync("ReceiveMessage", senderId, message);

        }
    }
}
