using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NuGet.Versioning;
using prjCoreWantMember.ViewModels;
using prjCoreWebWantWant.Models;
using prjCoreWebWantWant.ViewModels;
using System.Text.Json;
using X.PagedList;
using CDictionary = prjCoreWebWantWant.Models.CDictionary;

namespace WantTask.Controllers
{
    public class ForumController : Controller
    {

        private readonly NewIspanProjectContext _db;

        private readonly IMemoryCache _memoryCache;
        public ForumController(IMemoryCache memoryCache, NewIspanProjectContext dbContext)
        {
            _memoryCache = memoryCache;
            _db = dbContext;
        }

        // GET: Forum
        public IActionResult CategoryList()
        {
            IEnumerable<ForumCategory> datas = from f in _db.ForumCategories
                                               .Include(f=>f.ForumPostCategories).ThenInclude(fp=>fp.Post)
                                               select f;
            return View(datas);
        }



        public IActionResult PostList(int? categoryId, string OrderBy, string q, int page = 1)
        {
            ViewBag.CurrentSort = OrderBy;
            ViewBag.CurrentQ = q;


            if (categoryId == null)
                return RedirectToAction("CategoryList");

            var posts = from p in _db.ForumPosts
                        .Include(p => p.ForumPostCategories).ThenInclude(pc => pc.Category)
                        .Include(p => p.InverseParent)
                        .Include(p => p.Account)
                        //.Include(p => p.ForumPostComments).ThenInclude(c => c.StatusNavigation)
                        //.Include(p => p.ForumPostComments).ThenInclude(c => c.Account)
                        //where p.ForumPostCategories.FirstOrDefault().CategoryId == categoryId
                        .Where(p=>p.ForumPostCategories.FirstOrDefault().CategoryId == categoryId)
                        where p.Status == 1 || p.Status == 4
                        where p.Parent == null
                        select p;


            if (!string.IsNullOrEmpty(q))
            {
                posts = posts.Where(p => p.Account.UserName.Contains(q)
                               || p.Title.Contains(q)
                               || p.PostContent.Contains(q)
                );
            }

            switch (OrderBy)
            {
                case "Date":
                    posts = posts.OrderBy(p => p.Created);
                    break;
                case "Date_desc":
                    posts = posts.OrderByDescending(p => p.Created);
                    break;
                default:
                    posts = posts.OrderByDescending(p => p.Created);
                    break;
            }

            ViewBag.CategoryId = categoryId;
            ViewBag.CategoryTitle = _db.ForumCategories.FirstOrDefault(c => c.CategoryId == categoryId)?.Title;

            ForumPostListModel viewmodel = new ForumPostListModel();
            int pageSize = 10;

            var postIDs = posts.Select(p => p.PostId).ToList();
            var replyCounts = _db.ForumPosts
                                .Where(p => postIDs.Contains(p.ParentId ?? 0) && p.Status != 2)
                                .GroupBy(p => p.ParentId)
                                .ToDictionary(g => g.Key ?? 0, g => g.Count());

            viewmodel.ReplyCounts = replyCounts;
            viewmodel.PagedPosts = (IPagedList<ForumPost>)posts.ToPagedList(page, pageSize);
            return View(viewmodel);


        }




        public IActionResult PostView(int? id)
        {
            if (id == null)
                return RedirectToAction("PostList");

            var post = _db.ForumPosts
                        .Include(p => p.ForumPostCategories).ThenInclude(pc => pc.Category)
                        .Include(p => p.Account)
                        .Where(p => p.ParentId == null && (p.Status == 1 || p.Status == 4))
                        .FirstOrDefault(p => p.PostId == (int)id);

            var replies = _db.ForumPosts
                        .Include(p => p.Account)
                        .Where(p => p.ParentId == id && (p.Status != 2))
                        .ToList();

            var postComment = _db.ForumPostComments
                        .Include(c => c.Account)
                        .Where(c => c.PostId == id && (c.Status == 1 || c.Status == 4))
                        .ToList();


            var postReplyList = new List<List<ForumPostComment>>();//抓每個回覆的留言
            if (replies.FirstOrDefault() != null)
            {
                var replyPostIds = replies.Select(r => r.PostId).ToList();//從replies抓所有ID

                foreach (var postId in replyPostIds)
                {
                    var postcomm = _db.ForumPostComments
                        .Include(p => p.Account)
                        .Where(p => p.PostId == postId && (p.Status == 1 || p.Status == 4))
                        .ToList();
                    postReplyList.Add(postcomm);
                }
            }

            //-----------------------觀看次數-----------------------------
            int viewCount = 0;
            //建立MemoryCache，先確定是否有某篇文章的KEY存在
            if (_memoryCache.TryGetValue("ViewCount_" + id, out viewCount))
            {
                viewCount = (int)_memoryCache.Get("ViewCount_" + id);//有的話就抓key對應的value
            }
            else
            {
                // 如果沒有這個key從資料庫中取得觀看次數
                viewCount = (post.ViewCount.HasValue) ? post.ViewCount.Value : 0;

                viewCount++;
                post.ViewCount = viewCount;
                _db.SaveChanges();
                // 將觀看次數存入快取，設定快取時間，例如 1 小時
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddHours(1)
                };
                _memoryCache.Set("ViewCount_" + id, viewCount, cacheEntryOptions);
            }

            var viewModel = new ForumPostViewModel();
            viewModel.MainPost = post;
            viewModel.Replies = replies;
            viewModel.MainComments = postComment;
            viewModel.SecondCommentsList = postReplyList;
            //沒登入的情況
            viewModel.Member= null;
            //有登入的情況
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                string userDataJson = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
                CLoginUser loggedInUser = JsonSerializer.Deserialize<CLoginUser>(userDataJson);
                int memId = loggedInUser.AccountId;
                viewModel.Member = _db.MemberAccounts.Find(memId);
            }

            return View(viewModel);
        }



        public IActionResult CreatePost(int? id)
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //判斷是否有登入
            {
                string userDataJson = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
                CLoginUser loggedInUser = JsonSerializer.Deserialize<CLoginUser>(userDataJson);

                if (id == null)
                    return RedirectToAction("CategoryList");
                return View();
            }
            else
            {
                string BackToAction = "CreatePost";
                string BackToController = "Forum";
                string BackToId = id.ToString();
                return RedirectToAction("Login", "Member", new { BackToAction, BackToController, BackToId });
            }
        }
        [HttpPost]
        public IActionResult CreatePost(ForumPost post)
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //判斷是否有登入
            {
                string userDataJson = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
                CLoginUser loggedInUser = JsonSerializer.Deserialize<CLoginUser>(userDataJson);

                ForumPost x = new ForumPost();
                x.AccountId = loggedInUser.AccountId;
                x.Title = post.Title;
                x.PostContent = post.PostContent;
                x.Created = DateTime.Now;
                x.Status = 1;
                x.ViewCount = 0;
                _db.ForumPosts.Add(x);
                _db.SaveChanges();

                int categoryId = 0;
                int.TryParse(Request.Query["id"], out categoryId);

                ForumPostCategory category = new ForumPostCategory();
                category.PostId = x.PostId;
                category.CategoryId = categoryId;
                _db.ForumPostCategories.Add(category);
                _db.SaveChanges();
                return RedirectToAction("PostList", new { categoryId = categoryId });
            }
            else
                return RedirectToAction("Login", "Member");
        }




        public IActionResult ForumMS()
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //判斷是否有登入
            {
                string userDataJson = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
                CLoginUser loggedInUser = JsonSerializer.Deserialize<CLoginUser>(userDataJson);

                var postlist = _db.ForumPosts
                .Where(p => p.AccountId == loggedInUser.AccountId && (p.Status == 1 || p.Status == 2))
                .ToList();

                List<ForumMSViewModel> articlesList = new List<ForumMSViewModel>();
                foreach (var post in postlist)
                {
                    var viewModel = new ForumMSViewModel
                    {
                        PostId = post.PostId,
                        Title = post.Title,
                        AccountId = post.AccountId,
                        ParentId = post.ParentId,
                        PostContent = post.PostContent,
                        Created = post.Created,
                        Updated = post.Updated,
                        Status = post.Status,
                        ViewCount = post.ViewCount,
                        LikeCount = post.LikeCount
                    };
                    articlesList.Add(viewModel);
                }
                return View(articlesList);
            }
            else
                return RedirectToAction("Login", "Member");
        }


}
}