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
using DocumentFormat.OpenXml.Spreadsheet;
using Azure;

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
        int TopPageSize = 3;
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
                            SkillNames = string.Join(" | ", skills),
                            CertificateNames = string.Join(" | ", certificates)
                        };

            ViewBag.TotalCount = datas.Distinct().Count();
            ViewBag.MaxPrice = datas.Max(p => p.CommonPrice);
            
            int currentPage = page < 1 ? 1 : page;
            IEnumerable<CExpertSearchViewModel> result = datas.ToPagedList(currentPage, pageSize);
            return View(result);
        }
        public IActionResult ExpertSearch(CExpertKeyword vm, int page = 1)
        {
            NewIspanProjectContext db = new NewIspanProjectContext();
            IEnumerable<CExpertSearchViewModel> datas = null;
            //先抓到所有的datas
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
                        SkillNames = string.Join(" | ", skills),
                        CertificateNames = string.Join(" | ", certificates)
                    };

            //如果有關鍵字的話篩掉這個datas
            if (!string.IsNullOrEmpty(vm.txtKeyword))
            {
                datas = datas.Where(m => m.Name.ToUpper().Contains(vm.txtKeyword.ToUpper())
                    || m.SkillNames.ToUpper().Contains(vm.txtKeyword.ToUpper())
                    || m.CertificateNames.ToUpper().Contains(vm.txtKeyword.ToUpper())
                    || m.ResumeTitle.ToUpper().Contains(vm.txtKeyword.ToUpper()));
                //因為沒有cityName所以我不能篩選城市欸QQ
            }

            //如果有選擇證照的話篩掉這個datas
            if (!string.IsNullOrEmpty(vm.SelectedCertificate)&& !vm.SelectedCertificate.Contains("請先選擇"))
            {
                datas=datas.Where(m=>m.CertificateNames.ToUpper().Contains(vm.SelectedCertificate.ToUpper()));
            }
            //如果有選擇類別或是專長的話篩掉這個datas
            if (!string.IsNullOrEmpty(vm.SelectedSkill) && !vm.SelectedSkill.Contains("請先選擇"))
            {
                datas = datas.Where(m => m.SkillNames.ToUpper().Contains(vm.SelectedSkill.ToUpper()));
            }
            //如果有限制最高價的話，篩掉這個datas
            if (vm.SelectedMaxPrice.HasValue)
            {
                datas = datas.Where(m =>m.CommonPrice<=vm.SelectedMaxPrice);
            }

            int currentPage = page < 1 ? 1 : page;
            IEnumerable<CExpertSearchViewModel> result = datas.ToPagedList(currentPage, pageSize);
            return PartialView("_ResultsPartial", result);
        }
        public ActionResult SortByHis(CExpertKeyword vm, int page = 1)//熱門程度(點閱數大到小排列)
        {
            NewIspanProjectContext db = new NewIspanProjectContext();
            IEnumerable<CExpertSearchViewModel> datas = null;
            //先抓到所有的datas
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
                        SkillNames = string.Join(" | ", skills),
                        CertificateNames = string.Join(" | ", certificates)
                    };

            //如果有關鍵字的話篩掉這個datas
            if (!string.IsNullOrEmpty(vm.txtKeyword))
            {
                datas = datas.Where(m => m.Name.ToUpper().Contains(vm.txtKeyword.ToUpper())
                    || m.SkillNames.ToUpper().Contains(vm.txtKeyword.ToUpper())
                    || m.CertificateNames.ToUpper().Contains(vm.txtKeyword.ToUpper())
                     || m.ResumeTitle.ToUpper().Contains(vm.txtKeyword.ToUpper()));
                //因為沒有cityName所以我不能篩選城市欸QQ
            }

            //如果有選擇證照的話篩掉這個datas
            if (!string.IsNullOrEmpty(vm.SelectedCertificate) && !vm.SelectedCertificate.Contains("請先選擇"))
            {
                datas = datas.Where(m => m.CertificateNames.ToUpper().Contains(vm.SelectedCertificate.ToUpper()));
            }
            //如果有選擇類別或是專長的話篩掉這個datas
            if (!string.IsNullOrEmpty(vm.SelectedSkill) && !vm.SelectedSkill.Contains("請先選擇"))
            {
                datas = datas.Where(m => m.SkillNames.ToUpper().Contains(vm.SelectedSkill.ToUpper()));
            }
            //如果有限制最高價的話，篩掉這個datas
            if (vm.SelectedMaxPrice.HasValue)
            {
                datas = datas.Where(m => m.CommonPrice <= vm.SelectedMaxPrice);
            }

            datas = datas.OrderByDescending(r => r.HistoricalViews);
            int currentPage = page < 1 ? 1 : page;
            IEnumerable<CExpertSearchViewModel> result = datas.ToPagedList(currentPage, pageSize);
            return PartialView("_ResultsPartial", result);
        }
        public ActionResult SortByDate(CExpertKeyword vm, int page = 1)//更新時間新到舊
        {
            NewIspanProjectContext db = new NewIspanProjectContext();
            IEnumerable<CExpertSearchViewModel> datas = null;
            //先抓到所有的datas
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
                        SkillNames = string.Join(" | ", skills),
                        CertificateNames = string.Join(" | ", certificates)
                    };

            //如果有關鍵字的話篩掉這個datas
            if (!string.IsNullOrEmpty(vm.txtKeyword))
            {
                datas = datas.Where(m => m.Name.ToUpper().Contains(vm.txtKeyword.ToUpper())
                    || m.SkillNames.ToUpper().Contains(vm.txtKeyword.ToUpper())
                    || m.CertificateNames.ToUpper().Contains(vm.txtKeyword.ToUpper())
                     || m.ResumeTitle.ToUpper().Contains(vm.txtKeyword.ToUpper()));
                //因為沒有cityName所以我不能篩選城市欸QQ
            }

            //如果有選擇證照的話篩掉這個datas
            if (!string.IsNullOrEmpty(vm.SelectedCertificate) && !vm.SelectedCertificate.Contains("請先選擇"))
            {
                datas = datas.Where(m => m.CertificateNames.ToUpper().Contains(vm.SelectedCertificate.ToUpper()));
            }
            //如果有選擇類別或是專長的話篩掉這個datas
            if (!string.IsNullOrEmpty(vm.SelectedSkill) && !vm.SelectedSkill.Contains("請先選擇"))
            {
                datas = datas.Where(m => m.SkillNames.ToUpper().Contains(vm.SelectedSkill.ToUpper()));
            }
            //如果有限制最高價的話，篩掉這個datas
            if (vm.SelectedMaxPrice.HasValue)
            {
                datas = datas.Where(m => m.CommonPrice <= vm.SelectedMaxPrice);
            }

            datas = datas.OrderByDescending(r => r.DataModifyDate);
            int currentPage = page < 1 ? 1 : page;
            IEnumerable<CExpertSearchViewModel> result = datas.ToPagedList(currentPage, pageSize);
            return PartialView("_ResultsPartial", result);
        }
        public ActionResult SortByPrice(CExpertKeyword vm, int page = 1)//價格低到高
        {
            NewIspanProjectContext db = new NewIspanProjectContext();
            IEnumerable<CExpertSearchViewModel> datas = null;
            //先抓到所有的datas
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
                        SkillNames = string.Join(" | ", skills),
                        CertificateNames = string.Join(" | ", certificates)
                    };

            //如果有關鍵字的話篩掉這個datas
            if (!string.IsNullOrEmpty(vm.txtKeyword))
            {
                datas = datas.Where(m => m.Name.ToUpper().Contains(vm.txtKeyword.ToUpper())
                    || m.SkillNames.ToUpper().Contains(vm.txtKeyword.ToUpper())
                    || m.CertificateNames.ToUpper().Contains(vm.txtKeyword.ToUpper())
                     || m.ResumeTitle.ToUpper().Contains(vm.txtKeyword.ToUpper()));
                //因為沒有cityName所以我不能篩選城市欸QQ
            }

            //如果有選擇證照的話篩掉這個datas
            if (!string.IsNullOrEmpty(vm.SelectedCertificate) && !vm.SelectedCertificate.Contains("請先選擇"))
            {
                datas = datas.Where(m => m.CertificateNames.ToUpper().Contains(vm.SelectedCertificate.ToUpper()));
            }
            //如果有選擇類別或是專長的話篩掉這個datas
            if (!string.IsNullOrEmpty(vm.SelectedSkill) && !vm.SelectedSkill.Contains("請先選擇"))
            {
                datas = datas.Where(m => m.SkillNames.ToUpper().Contains(vm.SelectedSkill.ToUpper()));
            }
            //如果有限制最高價的話，篩掉這個datas
            if (vm.SelectedMaxPrice.HasValue)
            {
                datas = datas.Where(m => m.CommonPrice <= vm.SelectedMaxPrice);
            }

            datas = datas.OrderBy(r => r.CommonPrice);
            int currentPage = page < 1 ? 1 : page;
            IEnumerable<CExpertSearchViewModel> result = datas.ToPagedList(currentPage, pageSize);
            return PartialView("_ResultsPartial", result);
        }
        public JsonResult GetTotalCount(CExpertKeyword vm)
        {
            NewIspanProjectContext db = new NewIspanProjectContext();
            IEnumerable<CExpertSearchViewModel> datas = null;
            //先抓到所有的datass
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
                        SkillNames = string.Join(" | ", skills),
                        CertificateNames = string.Join(" | ", certificates)
                    };

            //如果有關鍵字的話篩掉這個datas
            if (!string.IsNullOrEmpty(vm.txtKeyword))
            {
                datas = datas.Where(m => m.Name.ToUpper().Contains(vm.txtKeyword.ToUpper())
                    || m.SkillNames.ToUpper().Contains(vm.txtKeyword.ToUpper())
                    || m.CertificateNames.ToUpper().Contains(vm.txtKeyword.ToUpper())
                     || m.ResumeTitle.ToUpper().Contains(vm.txtKeyword.ToUpper()));
                //因為沒有cityName所以我不能篩選城市欸QQ
            }

            //如果有選擇證照的話篩掉這個datas
            if (!string.IsNullOrEmpty(vm.SelectedCertificate) && !vm.SelectedCertificate.Contains("請先選擇"))
            {
                datas = datas.Where(m => m.CertificateNames.ToUpper().Contains(vm.SelectedCertificate.ToUpper()));
            }
            //如果有選擇類別或是專長的話篩掉這個datas
            if (!string.IsNullOrEmpty(vm.SelectedSkill) && !vm.SelectedSkill.Contains("請先選擇"))
            {
                datas = datas.Where(m => m.SkillNames.ToUpper().Contains(vm.SelectedSkill.ToUpper()));
            }   
            //如果有限制最高價的話，篩掉這個datas
            if (vm.SelectedMaxPrice.HasValue)
            {
                datas = datas.Where(m => m.CommonPrice <= vm.SelectedMaxPrice);
            }

            //真的開始計數
            int totalCount = datas.Distinct().Count();
            return Json(totalCount);
        }

        public IActionResult TopExpertPartial(int page=1)
        {
            NewIspanProjectContext db = new NewIspanProjectContext();
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

                    where r.IsExpertOrNot == true && r.CaseStatusId == 23 && r.OnTop > DateTime.Now

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
                        SkillNames = string.Join(" | ", skills),
                        CertificateNames = string.Join(" | ", certificates)
                    };

            int currentPage = page < 1 ? 1 : page;
            IEnumerable<CExpertSearchViewModel> result = datas.ToPagedList(currentPage, TopPageSize);
            return PartialView("_ExpertTopPartial", result);
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

