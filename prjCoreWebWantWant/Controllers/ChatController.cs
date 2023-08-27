using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using prjCoreWebWantWant.Models;
using prjCoreWebWantWant.ViewModels;
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

                ViewBag.chatWithId = id;
                ViewBag.currentLoginId = loggedInUser.AccountId;

                //我的頭像
                var myava = _db.MemberAccounts.Where(p => p.AccountId == loggedInUser.AccountId).Select(p => p.MemberPhoto).FirstOrDefault();
                byte[] userAvatarBytes = myava;
                string userAvatarBase64 = Convert.ToBase64String(userAvatarBytes);
                string userAvatarUrl = $"data:image/png;base64,{userAvatarBase64}";
                ViewBag.currentLoginAvatarUrl = userAvatarUrl;

                //對方的頭像
                var withava = _db.MemberAccounts.Where(p => p.AccountId == id).Select(p => p.MemberPhoto).FirstOrDefault();
                byte[] withUserAvatarBytes = withava;
                string withUserAvatarBase64 = Convert.ToBase64String(withUserAvatarBytes);
                string withUserAvatarUrl = $"data:image/png;base64,{withUserAvatarBase64}";
                ViewBag.chatWithUserAvatarUrl = withUserAvatarUrl;
                return View();
            }
            else
            {
                string BackToAction = "ChatSingle"; //目前的action名字
                string BackToController = "Chat";//目前的controller名字
                string BackToId = id.ToString();//目前的id位置
                return RedirectToAction("Login", "Member", new { BackToAction, BackToController, BackToId });
            }

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

        public IActionResult ChatTo(int id)
        {
            //if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //判斷是否有登入
            //{
            //    string userDataJson = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
            //    CLoginUser loggedInUser = JsonSerializer.Deserialize<CLoginUser>(userDataJson);


            //    //我的頭像
            //    var myava = _db.MemberAccounts.Where(p => p.AccountId == loggedInUser.AccountId).Select(p => p.MemberPhoto).FirstOrDefault();
            //    byte[] userAvatarBytes = myava;
            //    string userAvatarBase64 = Convert.ToBase64String(userAvatarBytes);
            //    string userAvatarUrl = $"data:image/png;base64,{userAvatarBase64}";

            //    //對方的頭像
            //    var withava = _db.MemberAccounts.Where(p => p.AccountId == id).Select(p => p.MemberPhoto).FirstOrDefault();
            //    byte[] withUserAvatarBytes = withava;
            //    string withUserAvatarBase64 = Convert.ToBase64String(withUserAvatarBytes);
            //    string withUserAvatarUrl = $"data:image/png;base64,{withUserAvatarBase64}";

            //    var chatModel= new CChatToViewModel
            //    {
            //        chatWithId = id,
            //        currentLoginId = loggedInUser.AccountId,
            //        currentLoginAvatarUrl = userAvatarUrl,
            //        chatWithUserAvatarUrl = withUserAvatarUrl
            //    };


            //    return PartialView("ChatTo", chatModel);
            //}
            //else
            //{
            //    string BackToAction = "ChatSingle"; //目前的action名字
            //    string BackToController = "Chat";//目前的controller名字
            //    string BackToId = id.ToString();//目前的id位置
            //    return RedirectToAction("Login", "Member", new { BackToAction, BackToController, BackToId });
            //}
            System.Diagnostics.Debug.WriteLine($"Received id: {id}");

            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //判斷是否有登入
            {
                string userDataJson = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
                CLoginUser loggedInUser = JsonSerializer.Deserialize<CLoginUser>(userDataJson);

                ViewBag.chatWithId = id;
                ViewBag.currentLoginId = loggedInUser.AccountId;

                //我的頭像
                var myava = _db.MemberAccounts.Where(p => p.AccountId == loggedInUser.AccountId).Select(p => p.MemberPhoto).FirstOrDefault();
                byte[] userAvatarBytes = myava;
                string userAvatarBase64 = Convert.ToBase64String(userAvatarBytes);
                string userAvatarUrl = $"data:image/png;base64,{userAvatarBase64}";
                ViewBag.currentLoginAvatarUrl = userAvatarUrl;

                //對方的頭像
                var withava = _db.MemberAccounts.Where(p => p.AccountId == id).Select(p => p.MemberPhoto).FirstOrDefault();
                byte[] withUserAvatarBytes = withava;
                string withUserAvatarBase64 = Convert.ToBase64String(withUserAvatarBytes);
                string withUserAvatarUrl = $"data:image/png;base64,{withUserAvatarBase64}";
                ViewBag.chatWithUserAvatarUrl = withUserAvatarUrl;

                var chatModel = new CChatToViewModel
                {
                    chatWithId = id,
                    currentLoginId = loggedInUser.AccountId,
                    currentLoginAvatarUrl = userAvatarUrl,
                    chatWithUserAvatarUrl = withUserAvatarUrl
                };
                return PartialView("ChatTo", chatModel);
            }
            else
            {
                string BackToAction = "ChatTo"; //目前的action名字
                string BackToController = "Chat";//目前的controller名字
                string BackToId = id.ToString();//目前的id位置
                return RedirectToAction("Login", "Member", new { BackToAction, BackToController, BackToId });
            }
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
