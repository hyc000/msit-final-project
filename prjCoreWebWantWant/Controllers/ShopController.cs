using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjCoreWebWantWant.Models;
using System.Text.Json;

namespace prjShop.Controllers
{
    public class ShopController : Controller
    {
        private readonly NewIspanProjectContext _context;
        private readonly IWebHostEnvironment _host = null;

        public ShopController(NewIspanProjectContext context, IWebHostEnvironment host)
        {
            _context = context;
            _host = host;
        }

        public IActionResult ExpertShop()
        {
            var q = _context.Products
                .Include(t => t.Category)
                .Where(p => p.Status == "上架" && p.CategoryId == 1);

            return View(q);
        }
        public IActionResult CaseShop()
        {
            

            int id =GetAccountID();
            if (id == null)
            {
            
            }
            var q = _context.Products
              .Include(t => t.Category)
              .Where(p => p.Status == "上架" && p.CategoryId == 2);
            return View(q);
        }
        public IActionResult CheckOut()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Order()
        {
            return View();
        }

        public int GetAccountID()
        {
            string userDataJson = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
            CLoginUser loggedInUser = JsonSerializer.Deserialize<CLoginUser>(userDataJson);
            int id = loggedInUser.AccountId; //抓登入者的id                                                                             
            return id;
        }

        public IActionResult loadCase() 
        {
        int id =GetAccountID() ;

            var cases= _context.TaskLists
                .Where(t =>t.AccountId==id&&t.PublishOrNot=="立刻上架")
                .Select(t =>new { t.CaseId, t.TaskTitle})
                .ToList();
            return Json(new { success = true, tasks = cases });
        }

        public IActionResult loadExpert()
        {
            int id = GetAccountID();
            var resume = _context.Resumes
                 .Include(r => r.ExpertResume)
                .Where(r => r.AccountId == id && r.IsExpertOrNot == true)
                .Select(r => new { r.ResumeId,r.ExpertResume.Introduction})
             .ToList();

            return Json(resume);

        }

    }
}
