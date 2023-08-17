using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using prjCoreWantMember.ViewModels;
using prjCoreWebWantWant.Models;
using X.PagedList;

namespace WantTask.Controllers
{
    public class ForumController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        public ForumController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        int loginID = 56;//先綁死ID登入
        NewIspanProjectContext _db = new NewIspanProjectContext();

        // GET: Forum
        public IActionResult CategoryList()
        {
            IEnumerable<ForumCategory> datas = from f in _db.ForumCategories
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
                        .Include(p => p.Account)
                        .Include(p => p.ForumPostComments).ThenInclude(c => c.StatusNavigation)
                        .Include(p => p.ForumPostComments).ThenInclude(c => c.Account)
                        where p.ForumPostCategories.FirstOrDefault().CategoryId == categoryId
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
            viewmodel.PagedPosts = posts.ToPagedList(page, pageSize);
            return View(viewmodel);


        }




        public IActionResult PostView(int? postID)
        {
            if (postID == null)
                return RedirectToAction("PostList");

            var post = _db.ForumPosts
                        .Include(p => p.ForumPostCategories).ThenInclude(pc => pc.Category)
                        .Include(p => p.Account)
                        .Where(p => p.ParentId == null && (p.Status == 1 || p.Status == 4))
                        .FirstOrDefault(p => p.PostId == (int)postID);

            var replies = _db.ForumPosts
                        .Include(p => p.Account)
                        .Where(p => p.ParentId == postID)
                        .Where(p => p.ParentId == postID && (p.Status != 2))
                        .ToList();

            var postComment = _db.ForumPostComments
                        .Include(c => c.Account)
                        .Where(c => c.PostId == postID && (c.Status == 1 || c.Status == 4))
                        .ToList();

            //todo 搜尋關鍵字後這邊會出錯

            List<ForumPostComment> postReply = new List<ForumPostComment>();
            if (replies.FirstOrDefault() != null)
            {
                postReply = _db.ForumPostComments
                         .Include(p => p.Account)
                         .Where(p => p.PostId == replies.FirstOrDefault().PostId)
                         .ToList();
            }





            //-----------------------觀看次數-----------------------------
            int viewCount = 0;
            //建立MemoryCache，先確定是否有某篇文章的KEY存在
            if (_memoryCache.TryGetValue("ViewCount_" + postID, out viewCount))
            {
                viewCount = (int)_memoryCache.Get("ViewCount_" + postID);//有的話就抓key對應的value
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
                _memoryCache.Set("ViewCount_" + postID, viewCount, cacheEntryOptions);
            }

            var viewModel = new ForumPostViewModel();
            viewModel.MainPost = post;
            viewModel.Replies = replies;
            viewModel.MainComments = postComment;
            viewModel.SecondComments = postReply;

            return View(viewModel);
        }



        public IActionResult CreatePost(int? categoryId)
        {
            if (categoryId == null)
                return RedirectToAction("CategoryList");
            return View();
        }
        [HttpPost]
        public IActionResult CreatePost(ForumPost post)
        {
            ForumPost x = new ForumPost();
            x.AccountId = loginID;
            x.Title = post.Title;
            x.PostContent = post.PostContent;
            x.Created = DateTime.Now;
            x.Status = 1;
            x.ViewCount = 0;
            _db.ForumPosts.Add(x);
            _db.SaveChanges();

            int categoryId = 0;
            int.TryParse(Request.Query["categoryId"], out categoryId);

            ForumPostCategory category = new ForumPostCategory();
            category.PostId = x.PostId;
            category.CategoryId = categoryId;
            _db.ForumPostCategories.Add(category);
            _db.SaveChanges();
            return RedirectToAction("PostList", new { categoryId = categoryId });
        }




        public IActionResult ForumMS()
        {
            var postlist = _db.ForumPosts.Include(p=>p.StatusNavigation).ToArray();
            return View(postlist);
        }

    }
}