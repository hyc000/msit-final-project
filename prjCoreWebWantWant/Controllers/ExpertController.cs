using Microsoft.AspNetCore.Mvc;
using prjCoreWebWantWant.Models;
using prjCoreWantMember.ViewModels;
using System.Text.Json;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace prjCoreWantMember.Controllers
{
    public class ExpertController : Controller
    {
        private readonly ILogger<ExpertController> _logger;

        public ExpertController(ILogger<ExpertController> logger)
        {
            _logger = logger;
        }
        public IActionResult ExpertMainPage(CKeywordViewModel vm)
        {
            NewIspanProjectContext db = new NewIspanProjectContext();

            IEnumerable<CExpertInfoViewModel> datas = null;
            if (string.IsNullOrEmpty(vm.txtKeyword))
            {
                datas = from r in db.Resumes
                        join m in db.MemberAccounts
                        on r.AccountId equals m.AccountId
                        join er in db.ExpertResumes
                       on r.ResumeId equals er.ResumeId
                       join rSk in db.ResumeSkills
                       on r.ResumeId equals rSk.ResumeId
                       join rCe in db.ResumeCertificates
                       on r.ResumeId equals rCe.ResumeId
                        where r.IsExpertOrNot == true
                        select new CExpertInfoViewModel { resume = r, memberAccount = m, expertResume = er, resumeskill=rSk, resumecertificate=rCe };
            }
            else
            {
                datas = from r in db.Resumes
                        join m in db.MemberAccounts
                       on r.AccountId equals m.AccountId
                        join er in db.ExpertResumes
                        on r.ResumeId equals er.ResumeId
                        where r.IsExpertOrNot == true && (m.Name.ToUpper().Contains(vm.txtKeyword.ToUpper()) ||
                        r.Address.ToUpper().Contains(vm.txtKeyword.ToUpper()))
                        select new CExpertInfoViewModel { resume = r, memberAccount = m, expertResume = er };
            }
            return View(datas);
        }
        public IActionResult ExpertMemberPage()
        {
            NewIspanProjectContext db = new NewIspanProjectContext();
            string userDataJson = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
            MemberAccount loggedInUser = JsonSerializer.Deserialize<MemberAccount>(userDataJson); //loggedInUser的資料型態為MemberAccount這個資料表
                                                                                                  // 现在 loggedInUser 对象包含了从会话中取出的用户信息

            int id = loggedInUser.AccountId; //抓登入者的id
            IEnumerable<CExpertInfoViewModel> datas = null;
            datas = from r in db.Resumes
                    join m in db.MemberAccounts
                    on r.AccountId equals m.AccountId
                    join er in db.ExpertResumes
                   on r.ResumeId equals er.ResumeId
                    where r.IsExpertOrNot == true && r.AccountId == id
                    select new CExpertInfoViewModel { resume = r, memberAccount = m, expertResume = er };
            if (datas.Count() == 0)
            {
                datas = from m in db.MemberAccounts
                        where m.AccountId == id
                        select new CExpertInfoViewModel { memberAccount = m };
            }
            return View(datas);

        }

        public IActionResult EditExpertResume(int? id)
        {
            NewIspanProjectContext db = new NewIspanProjectContext();

            IEnumerable<CExpertInfoViewModel> datas = null;
            datas = from r in db.Resumes
                    join m in db.MemberAccounts
                    on r.AccountId equals m.AccountId
                    join er in db.ExpertResumes
                   on r.ResumeId equals er.ResumeId
                    where r.IsExpertOrNot == true && r.ResumeId == id
                    select new CExpertInfoViewModel { resume = r, memberAccount = m, expertResume = er };
            return View(datas);
        }
        public IActionResult AddExpertResume(int? id)
        {
            NewIspanProjectContext db = new NewIspanProjectContext();
            CExpertInfoViewModel data = null;
            //data.resume.AccountId = (int)idforAccount;
            data = (from m in db.MemberAccounts
                    where m.AccountId == id
                    select new CExpertInfoViewModel { memberAccount = m }).FirstOrDefault();
            return View(data);
        }
    }

}

