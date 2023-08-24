using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using prjCoreWebWantWant.Models;
using System.Text.Json;

namespace prjCoreWebWantWant.Hubs
{
    public class ChatHub:Hub
    {
        private static List<CChatUserInfo> userConnections = new List<CChatUserInfo>();

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
            string connectionId = getUserConnId(receiverId);
            await Clients.Client(connectionId).SendAsync("ReceiveMessage", senderId, message);

        }

        private string getUserConnId(int receiverId)
        {
            var user = userConnections.Find(u => u.AccountId == receiverId);
                if (!string.IsNullOrEmpty(user.ConnectionId))
                {
                    return user.ConnectionId;
                }
                else { return "No Find."; }
        }

        public async Task UpdateUserInfo(int senderId,string vconnectionId)
        {
            // 獲取使用者的 accountId 和 ConnectionId
            int accountId = senderId;
            string connectionId = Context.ConnectionId;

            CChatUserInfo existingUserInfo = userConnections.FirstOrDefault(u => u.AccountId == accountId);
            if (existingUserInfo != null)
            {
                // 更新現有的 ConnectionId
                existingUserInfo.ConnectionId = connectionId;
            }
            else
            {
                // 新增新的使用者資訊
                CChatUserInfo userInfo = new CChatUserInfo
                {
                    AccountId = accountId,
                    ConnectionId = connectionId
                };
                userConnections.Add(userInfo);
            }

            await Clients.All.SendAsync("UpdateUserInfo", accountId, connectionId);
        }

    }
}
