using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using prjCoreWebWantWant.Models;
using System.Text.Json;

namespace prjCoreWebWantWant.Hubs
{
    public class CExpertTask : Hub
    {
        private readonly NewIspanProjectContext _db;
        public CExpertTask(NewIspanProjectContext dbContext)
        {
            _db = dbContext;
        }
        //public async Task SendUpdateNotification(string divId,string message)
        //{
        //    await Clients.All.SendAsync("ReceiveUpdate", divId, message);
        //}
        public async Task SetClientInfo(string userId)
        {
            var currentId = Context.ConnectionId;
            ConnectionManager.AddConnection(userId, currentId);
        }
        // 例如：在 Hub 中保存用戶的 connectionId
        public override async Task OnConnectedAsync()
        {
            string userId = Context.UserIdentifier;

            // 檢查userId是否存在，如果存在則加入連接管理
            if (!string.IsNullOrEmpty(userId))
            {
                ConnectionManager.AddConnection(userId, Context.ConnectionId);
            }

            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Context.UserIdentifier;
            ConnectionManager.RemoveConnection(userId);
            await base.OnDisconnectedAsync(exception);
        }


        //public async Task NotifyUserBAboutChange(string userBId, string divId, string message)
        //{
        //    // 從之前保存的資料結構中獲取 userB 的 connectionId
        //    string targetConnectionId = ConnectionManager.GetConnectionId(userBId);

        //    if (!string.IsNullOrEmpty(targetConnectionId))
        //    {
        //        await Clients.Client(targetConnectionId).SendAsync("ReceiveUpdate", divId, message);
        //    }
        //}

    }
}
