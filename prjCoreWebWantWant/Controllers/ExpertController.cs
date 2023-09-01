using Microsoft.AspNetCore.Mvc;
using prjCoreWebWantWant.Models;
using prjCoreWantMember.ViewModels;
using System.Text.Json;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;
using X.PagedList.Mvc.Core;
using System.Collections.Generic;
using X.PagedList;
using prjCoreWebWantWant.ViewModels;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using Microsoft.Identity.Client;

namespace prjCoreWantMember.Controllers
{
    public class ExpertController : Controller
    {
        private readonly ILogger<ExpertController> _logger;

        public ExpertController(ILogger<ExpertController> logger)
        {
            _logger = logger;
        }
        int pageSize = 5;
        public IActionResult ExpertMainPage(int  page=1) //CKeywordViewModel vm, 
        {
            NewIspanProjectContext db = new NewIspanProjectContext();
            CExperTaskFactory factory = new CExperTaskFactory(db);

            IEnumerable<CExpertSearchViewModel> datas = null;
             datas = from r in db.Resumes
                        join m in db.MemberAccounts on r.AccountId equals m.AccountId
                        join er in db.ExpertResumes on r.ResumeId equals er.ResumeId

                     // Left Join with ResumeSkills and then with Skills for skill name
                     let skills = (from rSk in db.ResumeSkills
                                   join sk in db.Skills on rSk.SkillId equals sk.SkillId
                                   where r.ResumeId == rSk.ResumeId
                                   select sk.SkillName).ToList() // Convert to List

                     // Left Join with ResumeCertificates and then with Certificates for certificate name
                     let certificates = (from rCe in db.ResumeCertificates
                                         join ce in db.Certificates on rCe.CertificateId equals ce.CertificateId
                                         where r.ResumeId == rCe.ResumeId
                                         select ce.CertificateName).ToList() // Convert to List

                     where r.IsExpertOrNot == true && r.CaseStatusId == 23

                        select new CExpertSearchViewModel
                        {
                            AccountId = m.AccountId,
                            Name = m.Name,
                            ResumeId = r.ResumeId,
                            ResumeTitle = r.ResumeTitle,
                            DataModifyDate = r.DataModifyDate,
                            Photo = r.Photo,
                            TownId = r.TownId,
                            Introduction = er.Introduction,
                            ContactMethod = er.ContactMethod,
                            PaymentMethod = er.PaymentMethod,
                            CommonPrice = er.CommonPrice,
                            HistoricalViews = er.HistoricalViews,
                            SkillNames = string.Join(",", skills),
                            CertificateNames = string.Join(",", certificates)
                        };

            //}
            //else
            //{
            //    datas = from r in db.Resumes
            //            join m in db.MemberAccounts
            //           on r.AccountId equals m.AccountId
            //            join er in db.ExpertResumes
            //            on r.ResumeId equals er.ResumeId
            //            join rSk in db.ResumeSkills
            //          on r.ResumeId equals rSk.ResumeId
            //            join rCe in db.ResumeCertificates
            //            on r.ResumeId equals rCe.ResumeId
            //            where (r.IsExpertOrNot == true && r.CaseStatusId == 23) &&((m.Name.ToUpper().Contains(vm.txtKeyword.ToUpper()) ||
            //            r.Address.ToUpper().Contains(vm.txtKeyword.ToUpper())))
            //            select new CExpertInfoViewModel { resume = r, memberAccount = m, expertResume = er };
            //}
            ViewBag.TotalCount = datas.Distinct().Count();
            ViewBag.MaxPrice = datas.Max(p => p.CommonPrice);
            
            int currentPage = page < 1 ? 1 : page;
            IEnumerable<CExpertSearchViewModel> result = datas.ToPagedList(currentPage, pageSize);
            return View(result);
        }
        public IActionResult ExpertSearch(CKeywordViewModel vm, int page = 1)
        {
            NewIspanProjectContext db = new NewIspanProjectContext();
            CExperTaskFactory factory = new CExperTaskFactory(db);

            IEnumerable<CExpertSearchViewModel> datas = null;
            datas = from r in db.Resumes
                    join m in db.MemberAccounts on r.AccountId equals m.AccountId
                    join er in db.ExpertResumes on r.ResumeId equals er.ResumeId

                    // Left Join with ResumeSkills and then with Skills for skill name
                    let skills = (from rSk in db.ResumeSkills
                                  join sk in db.Skills on rSk.SkillId equals sk.SkillId
                                  where r.ResumeId == rSk.ResumeId
                                  select sk.SkillName).ToList() // Convert to List

                    // Left Join with ResumeCertificates and then with Certificates for certificate name
                    let certificates = (from rCe in db.ResumeCertificates
                                        join ce in db.Certificates on rCe.CertificateId equals ce.CertificateId
                                        where r.ResumeId == rCe.ResumeId
                                        select ce.CertificateName).ToList() // Convert to List

                    where (r.IsExpertOrNot == true && r.CaseStatusId == 23) &&
                    (m.Name.ToUpper().Contains(vm.txtKeyword.ToUpper()) || r.Address.ToUpper().Contains(vm.txtKeyword.ToUpper()))

                    select new CExpertSearchViewModel
                    {
                        AccountId = m.AccountId,
                        Name = m.Name,
                        ResumeId = r.ResumeId,
                        ResumeTitle = r.ResumeTitle,
                        DataModifyDate = r.DataModifyDate,
                        Photo = r.Photo,
                        TownId = r.TownId,
                        Introduction = er.Introduction,
                        ContactMethod = er.ContactMethod,
                        PaymentMethod = er.PaymentMethod,
                        CommonPrice = er.CommonPrice,
                        HistoricalViews = er.HistoricalViews,
                        SkillNames = string.Join(",", skills),
                        CertificateNames = string.Join(",", certificates)
                    };
          
            int currentPage = page < 1 ? 1 : page;
            IEnumerable<CExpertSearchViewModel> result = datas.ToPagedList(currentPage, pageSize);
            return PartialView("_ResultsPartial", result);
        }
        public JsonResult GetTotalCount(CKeywordViewModel vm)
        {
            NewIspanProjectContext db = new NewIspanProjectContext();
            CExperTaskFactory factory = new CExperTaskFactory(db);

            IEnumerable<CExpertSearchViewModel> datas = null;
            datas = from r in db.Resumes
                    join m in db.MemberAccounts on r.AccountId equals m.AccountId
                    join er in db.ExpertResumes on r.ResumeId equals er.ResumeId

                    // Left Join with ResumeSkills and then with Skills for skill name
                    let skills = (from rSk in db.ResumeSkills
                                  join sk in db.Skills on rSk.SkillId equals sk.SkillId
                                  where r.ResumeId == rSk.ResumeId
                                  select sk.SkillName).ToList() // Convert to List

                    // Left Join with ResumeCertificates and then with Certificates for certificate name
                    let certificates = (from rCe in db.ResumeCertificates
                                        join ce in db.Certificates on rCe.CertificateId equals ce.CertificateId
                                        where r.ResumeId == rCe.ResumeId
                                        select ce.CertificateName).ToList() // Convert to List

                    where (r.IsExpertOrNot == true && r.CaseStatusId == 23) &&
                    (m.Name.ToUpper().Contains(vm.txtKeyword.ToUpper()) || r.Address.ToUpper().Contains(vm.txtKeyword.ToUpper()))

                    select new CExpertSearchViewModel
                    {
                        AccountId = m.AccountId,
                        Name = m.Name,
                        ResumeId = r.ResumeId,
                        ResumeTitle = r.ResumeTitle,
                        DataModifyDate = r.DataModifyDate,
                        Photo = r.Photo,
                        TownId = r.TownId,
                        Introduction = er.Introduction,
                        ContactMethod = er.ContactMethod,
                        PaymentMethod = er.PaymentMethod,
                        CommonPrice = er.CommonPrice,
                        HistoricalViews = er.HistoricalViews,
                        SkillNames = string.Join(",", skills),
                        CertificateNames = string.Join(",", certificates)
                    };
            int totalCount = datas.Distinct().Count();
            return Json(totalCount);
        }

        public IActionResult ExpertMemberPage()
        {
            NewIspanProjectContext db = new NewIspanProjectContext();
            string userDataJson = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
            CLoginUser loggedInUser = JsonSerializer.Deserialize<CLoginUser>(userDataJson); //loggedInUser的資料型態為CLoginUser這個資料表
                                                                                               // 现在 loggedInUser 对象包含了从会话中取出的用户信息

            int id = loggedInUser.AccountId; //抓登入者的id
            IEnumerable<CExpertInfoViewModel> datas = null;
            datas = from r in db.Resumes
                    join m in db.MemberAccounts
                    on r.AccountId equals m.AccountId
                    join er in db.ExpertResumes
                   on r.ResumeId equals er.ResumeId
                    where r.IsExpertOrNot == true && r.AccountId == id &&r.CaseStatusId==23
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

        public IActionResult ExpertResumeDelete(int? id)
        {
            if (id != null)
            {
                NewIspanProjectContext db = new NewIspanProjectContext();
                Resume resume = db.Resumes.FirstOrDefault(p => p.ResumeId == id);
                if (resume != null)
                {
                    resume.CaseStatusId = 22;
                    //_context.Resumes.Remove(resume);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("ExpertMemberPage");
        }
    }

}

