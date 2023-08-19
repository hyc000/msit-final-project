using Microsoft.AspNetCore.Mvc;
using prjCoreWebWantWant.Models;
using prjCoreWebWantWant.ViewModels;
using System.Text.Json;

namespace prjCoreWantMember.Controllers
{
    public class MemberController : Controller
    {
        public IActionResult Index()
        {
            return View();
            //家個備註
        }
        public IActionResult MemberAccount(int? idforreal)
        {
            int id = 33; //還沒做登入先寫死
            NewIspanProjectContext db = new NewIspanProjectContext();
            MemberAccount datas = db.MemberAccounts.FirstOrDefault(p => (int)p.AccountId == (int)id);
            return View(datas);
        }
        public IActionResult EditMemberInfo(int? idforreal)
        {
            int id = 33; //還沒做登入先寫死
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
                return RedirectToAction("MemberAccount");
            }
            return View();
        }
        public IActionResult ForgetPassword()
        {
            return View();
        }
    }
}
