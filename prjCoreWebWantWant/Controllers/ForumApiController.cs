using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using prjCoreWantMember.ViewModels;
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


        public IActionResult PostCommit(ForumPostComment jsin)
        {
            ForumPostComment comment = new ForumPostComment();

            comment.AccountId = jsin.AccountId;
            comment.PostId = jsin.PostId;
            comment.Comment = jsin.Comment;
            comment.Created = DateTime.Now;
            comment.Status = 1;

            _db.ForumPostComments.Add(comment);
            _db.SaveChanges();

            return Content(comment.PostCommentId.ToString().Trim());
        }

        public IActionResult PostComment(int? id)//暫時沒用到
        {
            var vm = new ForumPostViewModel();
            vm.MainComments = _db.ForumPostComments
                .Include(c => c.Account)
                .Where(c => c.PostId == id && (c.Status == 1 || c.Status == 4))
                .ToList();

            return PartialView(vm);
        }

        public async Task<IActionResult> EditPost(ForumPost editin, int id)
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //判斷是否有登入
            {
                string userDataJson = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
                CLoginUser loggedInUser = JsonSerializer.Deserialize<CLoginUser>(userDataJson);

                ForumPost post = await _db.ForumPosts.FindAsync(id);

                if (post != null)
                {
                    // 更新文章內容及更新時間
                    post.Title = editin.Title;
                    post.PostContent = editin.PostContent;
                    post.Updated = DateTime.Now;

                    // 保存更改到數據庫
                    await _db.SaveChangesAsync();
                }

                return Json(new { success = true });

            }
            else
                return RedirectToAction("Login", "Member");
        }

        public IActionResult DeletePost(int? id)
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //判斷是否有登入
            {
                string userDataJson = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
                CLoginUser loggedInUser = JsonSerializer.Deserialize<CLoginUser>(userDataJson);

                ForumPost post =_db.ForumPosts.Find(id);

                if (post != null)
                {
                    post.Status = 3;
                    post.Updated = DateTime.Now;

                    // 保存更改到數據庫
                    _db.SaveChangesAsync();
                }

                return Json(new { success = true });

            }
            else
                return RedirectToAction("Login", "Member");
        }



    }
}

