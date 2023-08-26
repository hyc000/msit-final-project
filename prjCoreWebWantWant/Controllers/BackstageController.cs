using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using prjCoreWebWantWant.Models;
using prjCoreWebWantWant.ViewModels;
using System.Net.Http;
using System.Text.Json;

namespace prjWantWant_yh_CoreMVC.Controllers
{
    public class BackstageController : Controller
    {
        private readonly NewIspanProjectContext _context;

        private readonly IWebHostEnvironment _host;
        public BackstageController(NewIspanProjectContext context, IWebHostEnvironment host)
        {
            _context = context;
            _host = host;
        }

        public int GetAccountID()
        {
            string userDataJson = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
            CLoginUser loggedInUser = JsonSerializer.Deserialize<CLoginUser>(userDataJson); 
            int id = loggedInUser.AccountId; //抓登入者的id                                                                             
            return id;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Resume p,int selectedTownId, int selectedSkillId1,int selectedSkillId2,int selectedSkillId3, byte selectedPhoto)
        {
                p.TownId = selectedTownId;
                p.Photo = new byte[] {selectedPhoto};
                _context.Resumes.Add(p);
                _context.SaveChanges();

                 ResumeSkill resumeSkill1 = new ResumeSkill()
                {
                    ResumeId = p.ResumeId,
                    SkillId = selectedSkillId1
                };
                _context.Add(resumeSkill1);
                _context.SaveChanges();

                ResumeSkill resumeSkill2 = new ResumeSkill()
                {
                    ResumeId = p.ResumeId,
                    SkillId = selectedSkillId2
                };
                _context.Add(resumeSkill2);
                _context.SaveChanges();

                ResumeSkill resumeSkill3 = new ResumeSkill()
                {
                    ResumeId = p.ResumeId,
                    SkillId = selectedSkillId3
                };
                _context.Add(resumeSkill3);
                _context.SaveChanges();
                return RedirectToAction("ResumeList");
        }

        public IActionResult ResumeUneditable()
        {
            var q = _context.Resumes
                    .Where(r => r.IsExpertOrNot == false && r.AccountId == GetAccountID() && r.CaseStatusId != 22);
            return View(q);
        }

        public IActionResult ResumeDetail(int? id)
        {
            if (id == null)
                return RedirectToAction("ResumeList");
            Resume resume = _context.Resumes.FirstOrDefault(p => p.ResumeId == id);
            if (resume == null)
                return RedirectToAction("ResumeList");
            CResumeWrap resumeWrap = new CResumeWrap();
            resumeWrap.resume = resume;
            return View(resumeWrap);
        }
        [HttpPost]
        public ActionResult ResumeDetail(CTaskWrap pIn)
        {
            TaskList pDb = _context.TaskLists.FirstOrDefault(p => p.CaseId == pIn.FId);
            if (pDb != null)
            {
                pDb.CaseId = pIn.FId;
                pDb.TaskTitle = pIn.FTitle;
                pDb.TaskDetail = pIn.FDetail;
                pDb.PayFrom = pIn.FPayFrom;
                //db.SaveChanges();
            }
            return RedirectToAction("List");
        }

        public IActionResult ResumeList()
        {
            var q = _context.Resumes
                    .Include(t => t.Town.City)                                           
                    .Where(r => r.IsExpertOrNot == false && r.AccountId == GetAccountID() && r.CaseStatusId != 22);
            return View(q);
        }
        public IActionResult ResumeDelete(int? id)
        {
            if (id != null)
            {
                Resume resume = _context.Resumes.FirstOrDefault(p => p.ResumeId == id);
                if (resume != null)
                {
                    resume.CaseStatusId = 22;
                    //_context.Resumes.Remove(resume);
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("ResumeList");
        }

        //public IActionResult ResumeEdit()
        //{
        //    return View();
        //}

        public IActionResult ResumeEdit(int? id)
        {
            if (id == null)
                return RedirectToAction("ResumeList");
            Resume resume = _context.Resumes.FirstOrDefault(p => p.ResumeId == id);
            if (resume == null)
                return RedirectToAction("ResumeList");
            return View(resume);
        }

        [HttpPost]
        public IActionResult ResumeEdit(Resume pIn)
        {
            Resume resume = _context.Resumes.FirstOrDefault(p => p.ResumeId == pIn.ResumeId);
            if (resume != null)
            {
                resume.Address = pIn.Address;
                resume.AccountId = pIn.AccountId;
                _context.SaveChanges();
            }
            return RedirectToAction("ResumeList");
        }

        public IActionResult TaskCollection()
        {
            var q = from mc in _context.MemberCollections
                    join tl in _context.TaskLists on mc.CaseId equals tl.CaseId
                    where mc.AccountId == GetAccountID() && mc.CaseId != null
                    select new CMemberCollectionViewModel
                    {
                        TaskTitle = tl.TaskTitle,
                        TaskDetail = tl.TaskDetail,
                        RequiredNum = tl.RequiredNum,
                        PayFrom = tl.PayFrom,
                        TaskNameId =tl.TaskNameId,
                        PaymentId = tl.PaymentId,
                        CaseId = mc.CaseId,
                        CollectionDate = mc.CollectionDate
                    };

            return View(q.ToList());
        }

        public IActionResult DeleteCollection(int? id)
        {
            if (id != null)
            {
                MemberCollection memberCollection = _context.MemberCollections.FirstOrDefault(p => p.CaseId == id && p.AccountId == GetAccountID());
                if (memberCollection != null)
                {
                    _context.MemberCollections.Remove(memberCollection);
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("TaskCollection");
        }

        public IActionResult Apply(int? resumeId,int? caseId)
        {
            ApplicationList applicationList = new ApplicationList()
            {
                ResumeId = resumeId,
                CaseId = caseId,
                CaseStatusId = 21
            };
            _context.Add(applicationList);
            _context.SaveChanges(true);
            return RedirectToAction("TaskCollection");
        }

        public IActionResult ApplicationRecord()
        {
            var q = from al in _context.ApplicationLists
                    join tl in _context.TaskLists on al.CaseId equals tl.CaseId
                    where al.Resume.AccountId == GetAccountID() && al.Resume.ResumeId == al.ResumeId && al.CaseStatusId == 21
                    select new CMemberCollectionViewModel
                    {
                        TaskTitle = tl.TaskTitle,
                        TaskDetail = tl.TaskDetail,
                        RequiredNum = tl.RequiredNum,
                        PayFrom = tl.PayFrom,
                        TaskNameId = tl.TaskNameId,
                        PaymentId = tl.PaymentId,
                        CaseId = al.CaseId,
                        ApplicationDate = al.ApplicationDate
                    };

            return View(q.ToList());
        }

        public IActionResult GetTownId(string City ,string District)
        {
            var townId = _context.Cities
                         .Where(a => a.City1 == City)
                         .SelectMany(c => c.Towns)
                         .Where(c => c.Town1 == District)
                         .Select(c => c.TownId);

            return Json(townId);
        }

        //找出SkillId
        public IActionResult GetSkillId(string skillType ,string skillName)
        {
            var skillId = _context.SkillTypes
                         .Where(a => a.SkillTypeName == skillType)
                         .SelectMany(c => c.Skills)
                         .Where(c => c.SkillName == skillName)
                         .Select(c => c.SkillId);

            return Json(skillId);
        }
    }
}
