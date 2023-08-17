using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjCoreWebWantWant.Models;
using System.Linq;
using System.Security.Policy;
using WantTask.ViewModels;

namespace WantTask.Controllers
{
    public class ApiController : Controller
    {
        private readonly NewIspanProjectContext _context;

        private readonly IWebHostEnvironment _host;
        public ApiController(NewIspanProjectContext context, IWebHostEnvironment host)
        {
            _context = context;
            _host = host;
        }
        public IActionResult GetTaskPublished(CTaskListDetailItem taskList)
        {
            NewIspanProjectContext _context = new NewIspanProjectContext();
            IEnumerable<TaskList> datas = null;

            if ( datas !=null && taskList.FPublishOrNot != null)
            { 
                var tasklist = _context.TaskLists.Where(t=>t.PublishOrNot == "立刻上架").ToList() ;
                return View(tasklist);
            }

            return Content("No tasks found.");
        }

        public IActionResult Approve( CApproveViewModel approve)
        {

            var query = from app in _context.ApplicationLists
                        join task in _context.TaskLists on app.CaseId equals task.CaseId
                        join resume in _context.Resumes on task.TaskNameId equals resume.TaskNameId
                        join resumeskill in _context.ResumeSkills on resume.ResumeId equals resumeskill.ResumeId
                        join skill in _context.Skills on resumeskill.SkillId equals skill.SkillId
                        join resumecer in _context.ResumeCertificates on resume.ResumeId equals resumecer.ResumeId
                        join cer in _context.Certificates on resumecer.CertificateId equals cer.CertificateId
                        join member in _context.MemberAccounts on resume.AccountId equals member.AccountId
                        where app.CaseStatusId == 21
                        select new CApproveViewModel
                        {
                            CaseId = task.CaseId,
                            ResumeId = resume.ResumeId,
                            TaskNameId = task.TaskNameId,
                            CaseStatusId = app.CaseStatusId,
                            Name = member.Name,
                            SkillName = skill.SkillName,
                            CertificateName = cer.CertificateName,
                            Autobiography = resume.Autobiography,
                            PublishStart = task.PublishStart,
                            //TaskStart = task.TaskStart,
                            TaskTitle = task.TaskTitle,
                            TaskDetail = task.TaskDetail
                        };

            var viewModelList = query.ToList();

            return View(viewModelList);

        }

    }
}
