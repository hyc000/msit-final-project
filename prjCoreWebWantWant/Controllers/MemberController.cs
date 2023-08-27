using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using prjCoreWebWantWant.Controllers;
using prjCoreWebWantWant.Models;
using prjCoreWebWantWant.ViewModels;
using System;
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
                CLoginUser loggedInUser = JsonSerializer.Deserialize<CLoginUser>(userDataJson); //loggedInUser的資料型態為CLoginUser
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
        [HttpPost]
        public async Task<IActionResult> EditMemberInfo(IFormCollection formCollection)
        {
            // Extract values from the form collection
            int memberId = int.Parse(formCollection["AccountId"]);
            string newName = formCollection["Name"];
            string newEmail = formCollection["Email"];
            string newPassword = formCollection["Password"];
            string newGender = formCollection["Gender"];
            string newUserName = formCollection["UserName"];
            string newBitrhDay = formCollection["BitrhDay"];
            IFormFile newPhoto = formCollection.Files.FirstOrDefault();

            NewIspanProjectContext db = new NewIspanProjectContext();
            var existingMember = db.MemberAccounts.FirstOrDefault(m => m.AccountId == memberId);

            if (existingMember != null)
            {
                existingMember.Name = newName;
                existingMember.Email = newEmail;
                existingMember.Password = newPassword;
                existingMember.Gender = newGender;
                existingMember.UserName = newUserName;
                existingMember.BitrhDay = DateTime.Parse(newBitrhDay);

                if (newPhoto != null && newPhoto.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await newPhoto.CopyToAsync(memoryStream);
                        existingMember.MemberPhoto = memoryStream.ToArray();
                    }
                }

                db.SaveChanges();
                //更新Session中的資料
                string userDataJson = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
                CLoginUser loggedInUser = JsonSerializer.Deserialize<CLoginUser>(userDataJson); //loggedInUser的資料型態為CLoginUser
                                                                                                // 现在 loggedInUser 对象包含了从Session中取出的用户信息

                loggedInUser.Name = newName;
                loggedInUser.Email = newEmail;
                loggedInUser.Password = newPassword;
                // 序列化並存回 Session 中
                string updatedUserDataJson = JsonSerializer.Serialize(loggedInUser);
                HttpContext.Session.SetString(CDictionary.SK_LOGINED_USER, updatedUserDataJson);

                return RedirectToAction("MemberAccount");
            }

            return NotFound();
        }
        
        public IActionResult Login(string BackToAction, string BackToController,string BackToId)
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(CLoginViewModel vm,string BackToAction, string BackToController, string BackToId)
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
                    return RedirectToAction("List","BackstageManagement");
                else if(user.UserRole==1&& BackToAction.IsNullOrEmpty())
                    return RedirectToAction("MemberAccount");
                else
                {
                    
                    string url = Url.Action(BackToAction, BackToController);
                    url += "?id="+ BackToId;
                    return Redirect(url);
                    //以下是不傳遞參數才可以用的方法
                    //return RedirectToAction(BackToAction, BackToController);
                }


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
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(MemberAccount member)
        {

            NewIspanProjectContext db = new NewIspanProjectContext();
            MemberAccount newmember = new MemberAccount();

            if (newmember != null)
            {
                newmember.AccountId = 0;
                newmember.Name = member.Name;
                newmember.Email = member.Email;
                newmember.Password = member.Password;
                newmember.IdcardNo = member.IdcardNo;
                newmember.Gender = member.Gender;
                newmember.UserName = member.UserName;
                newmember.BitrhDay = member.BitrhDay;


                db.MemberAccounts.Add(newmember);
                db.SaveChanges();

                if (newmember.Email.Contains("@want.com")) //客服人員都用公司email註冊
                {
                    int memID = (from m in db.MemberAccounts
                                 where m.Email == newmember.Email
                                 select m.AccountId).FirstOrDefault();
                    MemberRoleConn memRole = new MemberRoleConn
                    {
                        AccountId = memID,
                        RoleId = 2 //客服人員
                    };
                    db.MemberRoleConns.Add(memRole);

                }
                else //不是客服的都會獲得基本會員身分
                {
                    int memID = (from m in db.MemberAccounts
                                 where m.Email == newmember.Email
                                 select m.AccountId).FirstOrDefault();
                    MemberRoleConn memRole = new MemberRoleConn
                    {
                        AccountId = memID,
                        RoleId = 1 //會員
                    };
                    db.MemberRoleConns.Add(memRole);
                }
                db.SaveChanges();
            }

            return RedirectToAction("Login");
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
