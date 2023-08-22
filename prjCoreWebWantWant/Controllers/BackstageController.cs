using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Create(Resume p,int selectedTownId, CResumeViewModel resumeViewModel)
        {
            p.TownId = selectedTownId;
            _context.Resumes.Add(p);
            _context.SaveChanges();
            return RedirectToAction("ResumeList");
        }

        public IActionResult ResumeUneditable()
        {
            return View();
        }

        public IActionResult ResumeList()
        {
            var q = _context.Resumes                                           
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
            //var q = from mc in _context.MemberCollections
            //        //join tl in _context.TaskLists on mc. equals tl.
            //        where mc.AccountId == 33 && mc.CaseId != null
            //        select new
            //        {
            //            mc.TaskList.CaseID
            //        }

           // var q2 = from mc in _context.MemberCollections
           //          from tl in _context.TaskLists
           //          where mc.AccountId == 33 && mc.CaseId != null
           //          select new
           //          {
           //              tl.TaskTitle,
           //              taskdetail = tl.TaskDetail.Substring(0, 15),
           //              tl.PayFrom,
           //          };
           //var viewModelList = q2.ToList();

            return View();
        }

        public IActionResult ApplicationRecord()
        {
            var q = from al in _context.ApplicationLists
                    where al.CaseStatusId == 21 && al.Resume.AccountId == GetAccountID()
                    select new
                    {
                        
                    };
            return View();
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
    }
}
