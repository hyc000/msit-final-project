using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using prjCoreWebWantWant.Models;
using System.Text.Json;

namespace WantTask.Controllers
{
    public class ChatController : Controller
    {
        private readonly NewIspanProjectContext _db;

        public ChatController(NewIspanProjectContext dbContext)
        {
            _db = dbContext;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //判斷是否有登入
            {
                string userDataJson = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
                CLoginUser loggedInUser = JsonSerializer.Deserialize<CLoginUser>(userDataJson);

                var ava = _db.MemberAccounts.Where(p => p.AccountId == loggedInUser.AccountId).Select(p => p.MemberPhoto).FirstOrDefault();
                byte[] userAvatarBytes = ava;
                string userAvatarBase64 = Convert.ToBase64String(userAvatarBytes);
                string userAvatarUrl = $"data:image/png;base64,{userAvatarBase64}";
                ViewBag.currentLoginAvatarUrl = userAvatarUrl;
                ViewBag.currentLoginId = loggedInUser.AccountId;
                return View();
            }
            else
                return RedirectToAction("Login", "Member");
        }

        public IActionResult ChatSingle(int id)
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //判斷是否有登入
            {
                string userDataJson = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
                CLoginUser loggedInUser = JsonSerializer.Deserialize<CLoginUser>(userDataJson);
                var ava = _db.MemberAccounts.Where(p => p.AccountId == loggedInUser.AccountId).Select(p => p.MemberPhoto).FirstOrDefault();
                byte[] userAvatarBytes = ava;
                string userAvatarBase64 = Convert.ToBase64String(userAvatarBytes);
                string userAvatarUrl = $"data:image/png;base64,{userAvatarBase64}";
                ViewBag.chatWithId = id;
                ViewBag.currentLoginAvatarUrl = userAvatarUrl;
                ViewBag.currentLoginId = loggedInUser.AccountId;
                return View();
            }
            else
                return RedirectToAction("Login", "Member");
        }

        public IActionResult ChatMS()
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //判斷是否有登入
            {
                string userDataJson = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
                CLoginUser loggedInUser = JsonSerializer.Deserialize<CLoginUser>(userDataJson);

                var chatManager = _db.ChatMessages.ToArray();
                return View(chatManager);
            }
            else
                return RedirectToAction("Login", "Member");
        }

        public IActionResult ChatIcon()
        {

            return PartialView();
        }

        public IActionResult ChatTest(int id)
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //判斷是否有登入
            {
                string userDataJson = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
                CLoginUser loggedInUser = JsonSerializer.Deserialize<CLoginUser>(userDataJson);
                var ava = _db.MemberAccounts.Where(p => p.AccountId == loggedInUser.AccountId).Select(p => p.MemberPhoto).FirstOrDefault();
                byte[] userAvatarBytes = ava;
                string userAvatarBase64 = Convert.ToBase64String(userAvatarBytes);
                string userAvatarUrl = $"data:image/png;base64,{userAvatarBase64}";
                ViewBag.chatWithId = id;
                ViewBag.currentLoginAvatarUrl = userAvatarUrl;
                ViewBag.currentLoginId = loggedInUser.AccountId;
                return View();
            }
            else
                return RedirectToAction("Login", "Member");
        }

    }
}
