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
                CLoginUser loggedInUser = JsonSerializer.Deserialize<CLoginUser>(userDataJson); //loggedInUser的資料型態為MemberAccount這個資料表
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
            CLoginUser user = db.MemberAccounts.Where(
                m => m.Email.Equals(vm.txtAccount)).Select(m => new CLoginUser
                {
                    AccountId = m.AccountId,
                    Name=m.Name,
                    Email = m.Email,
                    Password = m.Password
                }).FirstOrDefault();
            if (user == null)
            {
                //想要跳通知提醒:請先註冊密碼
                return RedirectToAction("Register");
            }
            if (user != null && user.Password.Equals(vm.txtPassword))
            {
                string json = JsonSerializer.Serialize(user);
                HttpContext.Session.SetString(CDictionary.SK_LOGINED_USER, json);
                LoginHistory loghis = new LoginHistory();
                CLoginUser.setLogHis(loghis,user.AccountId,user.LastFailCount, 0, true);
                db.LoginHistories.Add(loghis);
                db.SaveChanges();
                if (user.UserRole==2)
                    //客服人員
                    return RedirectToAction("Index","BackstageManagement");
                else if(user.UserRole==1)
                    return RedirectToAction("MemberAccount");
            }
            else
            {
                LoginHistory loghis = new LoginHistory();
                CLoginUser.setLogHis(loghis, user.AccountId, user.LastFailCount, 1, false);
                db.LoginHistories.Add(loghis);
                db.SaveChanges();
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
