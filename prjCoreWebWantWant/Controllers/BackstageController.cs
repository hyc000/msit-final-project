using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using prjCoreWebWantWant.Models;


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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Resume p)
        {
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
            var q = _context.Resumes                                   //李芷帆         
                    .Where(r => r.IsExpertOrNot == false && r.AccountId == 33 && r.CaseStatusId != 22);
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

        public IActionResult ResumeEdit()
        {
            return View();
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
                    where al.CaseStatusId == 21 && al.Resume.AccountId == 33
                    select new
                    {
                        
                    };
            return View();
        }
    }
}
