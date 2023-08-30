using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using prjCoreWebWantWant.Models;
using prjCoreWebWantWant.ViewModels;
using System.Diagnostics;
using System.Text.Json;

namespace prjCoreWebWantWant.Controllers
{
    public class ForumApiController : Controller
    {

        private readonly NewIspanProjectContext _db;

        public ForumApiController(NewIspanProjectContext dbContext)
        {
            _db = dbContext;
        }



        public IActionResult PostReply(ForumPostReplyViewModel vm)
        {
            ForumPost reply = new ForumPost();

            reply.AccountId = vm.AccountId;
            reply.ParentId = vm.ParentId;
            reply.PostContent = vm.PostContent;
            reply.Created = DateTime.Now;
            reply.Status = 1;
            reply.ViewCount = 0;

            _db.ForumPosts.Add(reply);
            _db.SaveChanges();

            return Content(reply.PostId.ToString().Trim());
        }
    }
}
