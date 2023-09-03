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
        public async Task SendUpdateNotification(string message)
        {
            await Clients.All.SendAsync("ReceiveUpdate", message);
        }
    }
}
