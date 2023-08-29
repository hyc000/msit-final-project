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

        [HttpPost]
        public IActionResult PostReply(ForumPostReplyViewModel vm)
        {
            string userDataJson = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
            CLoginUser loggedInUser = JsonSerializer.Deserialize<CLoginUser>(userDataJson);

            ForumPost reply = new ForumPost();

            reply.AccountId = loggedInUser.AccountId;
            reply.Title = vm.ParentId.ToString();
            reply.ParentId = vm.ParentId;
            reply.PostContent = vm.PostContent;
            reply.Created= DateTime.Now;
            reply.Status = 1;
            reply.ViewCount = 0;

            _db.ForumPosts.Add(reply);
            _db.SaveChanges();

            int postId = Convert.ToInt32(vm.ParentId);

            Debug.WriteLine("到底有沒有來這");

            return RedirectToAction("postview", new {id= postId});
        }
    }
}
