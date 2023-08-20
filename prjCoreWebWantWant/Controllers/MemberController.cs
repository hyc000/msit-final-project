using Microsoft.AspNetCore.Mvc;
using prjCoreWebWantWant.Controllers;
using prjCoreWebWantWant.Models;
using prjCoreWebWantWant.ViewModels;
using System.Text.Json;

namespace prjCoreWantMember.Controllers
{
    public class MemberController : Controller
    {
        private readonly ILogger<MemberController> _logger;

        public MemberController(ILogger<MemberController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
            //家個備註
        }
        public IActionResult MemberAccount()
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //判斷是否有登入
            {
                string userDataJson = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
                    MemberAccount loggedInUser = JsonSerializer.Deserialize<MemberAccount>(userDataJson); //loggedInUser的資料型態為MemberAccount這個資料表
                // 现在 loggedInUser 对象包含了从会话中取出的用户信息

                int id = loggedInUser.AccountId; //抓登入者的id
                NewIspanProjectContext db = new NewIspanProjectContext();
                MemberAccount datas = db.MemberAccounts.FirstOrDefault(p => (int)p.AccountId == (int)id);
                return View(datas);
            }
              else
                return RedirectToAction("Login");

        }
        public IActionResult EditMemberInfo(int? id)
        {
            NewIspanProjectContext db = new NewIspanProjectContext();
            MemberAccount datas = db.MemberAccounts.FirstOrDefault(p => (int)p.AccountId == (int)id);
            return View(datas);
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(CLoginViewModel vm)
        {
            NewIspanProjectContext db = new NewIspanProjectContext();
            MemberAccount user = db.MemberAccounts.FirstOrDefault(
                m => m.Email.Equals(vm.txtAccount) && m.Password.Equals(vm.txtPassword));
            if (user != null && user.Password.Equals(vm.txtPassword))
            {
                string json = JsonSerializer.Serialize(user);
                HttpContext.Session.SetString(CDictionary.SK_LOGINED_USER, json);
                if (vm.txtAccount.Contains("want.com"))
                    return RedirectToAction("Index","BackstageManagement");
                else
                    return RedirectToAction("MemberAccount");
            }
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove(CDictionary.SK_LOGINED_USER);
            return RedirectToAction("Login");
        }
        public IActionResult ForgetPassword()
        {
            return View();
        }
    }
}
