using Microsoft.AspNetCore.Mvc;
using prjCoreWebWantWant.Models;

namespace WantTask.Controllers
{
    public class ChatController : Controller
    {
        int loginID = 56;//先綁死ID登入
        NewIspanProjectContext _db = new NewIspanProjectContext();

        public IActionResult Index()
        {
            var ava= _db.MemberAccounts.Where(p => p.AccountId == loginID).Select(p => p.MemberPhoto).FirstOrDefault();
            byte[] userAvatarBytes = ava;
            string userAvatarBase64 = Convert.ToBase64String(userAvatarBytes);
            string userAvatarUrl = $"data:image/png;base64,{userAvatarBase64}";
            ViewBag.currentLoginAvatarUrl = userAvatarUrl;
            ViewBag.currentLoginId=loginID;
            return View();
        }

        public IActionResult ChatSingle(int id)
        {
            var ava = _db.MemberAccounts.Where(p => p.AccountId == loginID).Select(p => p.MemberPhoto).FirstOrDefault();
            byte[] userAvatarBytes = ava;
            string userAvatarBase64 = Convert.ToBase64String(userAvatarBytes);
            string userAvatarUrl = $"data:image/png;base64,{userAvatarBase64}";
            ViewBag.chatWithId = id;
            ViewBag.currentLoginAvatarUrl = userAvatarUrl;
            ViewBag.currentLoginId = loginID;
            return View();
        }

        public IActionResult ChatMS()
        {
            var chatManager = _db.ChatMessages.ToArray();
            return View(chatManager);
        }
    }
}
