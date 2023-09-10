using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using prjCoreWantMember.ViewModels;
using prjCoreWebWantWant.Hubs;
using prjCoreWebWantWant.Models;
using prjCoreWebWantWant.ViewModels;
using System.Text.Json;

namespace prjCoreWebWantWant.Controllers
{
  
    public class ExpertTaskApiController : Controller
    {
        private readonly NewIspanProjectContext _context;
        private readonly IHubContext<CExpertTask> _hubContext;
        private int _memberID;
        private string _memberName;
        private readonly IWebHostEnvironment _env;
        public ExpertTaskApiController(NewIspanProjectContext context, IWebHostEnvironment env , IHubContext<CExpertTask> hubContext)
        {
            _hubContext = hubContext;
            _context = context;
            _env = env;

        }

        private int GetMemberIDFromSession()
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                string userDataJson = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
                MemberAccount loggedInUser = JsonSerializer.Deserialize<MemberAccount>(userDataJson);
                return loggedInUser.AccountId;
            }
            return 0;
        }

        public async Task<IActionResult> Mytasking()
        {
            _memberID = GetMemberIDFromSession();//登入者我自己memberID
            if (_memberID == 0)
            {
                TempData["message"] = "請先登入";
                return RedirectToAction("Login", "Member");
            }
            CExpertTaskListViewModel vw = new CExpertTaskListViewModel();
            CExperTaskFactory factory = new CExperTaskFactory(_context);

            CExpertTaskViewModel cexperttask;
            List<CExpertTaskViewModel> list = new List<CExpertTaskViewModel>();
            var q1 = await  _context.TaskLists
   .Include(b => b.ExpertApplications)
   .Where(x => x.IsExpert == true && x.ExpertApplications.Any(y => y.AccountId == _memberID && (y.CaseStatusId == 15 || y.CaseStatusId == 17)))
   .OrderBy(x => x.TaskEndDate)
   .Select(x => new
   {
       TaskList = x,
       ExpertApplications = x.ExpertApplications
                            .Where(ea => ea.AccountId == _memberID && (ea.CaseStatusId == 15 || ea.CaseStatusId == 17))
                            .Select(ea => new { ea.CaseStatusId, ea.AccountId })
   })
   .ToListAsync();
            foreach (var item in q1)
            {
                cexperttask = new CExpertTaskViewModel();
                var taskList = item.TaskList;
                foreach (var expertApplication in item.ExpertApplications)
                {
                    cexperttask.taskmember = factory.MemberName(expertApplication.AccountId);
                    cexperttask.taskmemberid = expertApplication.AccountId.GetValueOrDefault();

                    cexperttask.CaseStatusname = factory.StatusName(expertApplication.CaseStatusId);
                    // 使用上述的變數進行其他操作
                }

                cexperttask.taskexpert = factory.MemberName(taskList.AccountId);
                cexperttask.taskexpertid = taskList.AccountId.GetValueOrDefault();
                cexperttask.caseid = taskList.CaseId;
                cexperttask.taskcontent = taskList.TaskDetail;
                cexperttask.taskdatestart = taskList.TaskStartDate;
                cexperttask.taskdateend = taskList.TaskEndDate;
                cexperttask.taskprice = taskList.PayFrom;
                cexperttask.WorkPlace = (taskList.WorkPlace.GetValueOrDefault() ? "在家工作" : "指定地點工作");
                if (!taskList.WorkPlace.GetValueOrDefault())
                {
                    cexperttask.Address = taskList.Address;
                }
                else
                {
                    cexperttask.Address = "無";
                }
                list.Add(cexperttask);
            }
            return Json(list);
        }

        public async Task<IActionResult> Mytasked()
        {
            _memberID = GetMemberIDFromSession();//登入者我自己memberID
            if (_memberID == 0)
            {
                TempData["message"] = "請先登入";
                return RedirectToAction("Login", "Member");
            }
           
            CExperTaskFactory factory = new CExperTaskFactory(_context);
            CExpertTaskViewModel cexperttask;
            List<CExpertTaskViewModel> list2 = new List<CExpertTaskViewModel>();
            //mytasked
            //CaseId:16=專家拒絕接案，18=專家案件已完成 13=放棄委託//12專家案件已評價
            var q2 = _context.TaskLists
    .Include(b => b.ExpertApplications)
    .Where(x => x.IsExpert == true && x.ExpertApplications.Any(y => y.AccountId == _memberID && (y.CaseStatusId == 16 || y.CaseStatusId == 18 || y.CaseStatusId == 13 || y.CaseStatusId == 12 || y.CaseStatusId == 11)))
    .OrderBy(x => x.TaskEndDate)
    .Select(x => new
    {
        TaskList = x,
        ExpertApplications = x.ExpertApplications
                             .Where(ea => ea.AccountId == _memberID && (ea.CaseStatusId == 16 || ea.CaseStatusId == 18 || ea.CaseStatusId == 13 || ea.CaseStatusId == 12 || ea.CaseStatusId == 11))
                             .Select(ea => new { ea.CaseStatusId, ea.AccountId })
    })
    .ToList();

            foreach (var item in q2)
            {
                cexperttask = new CExpertTaskViewModel();
                var taskList = item.TaskList;
                foreach (var expertApplication in item.ExpertApplications)
                {
                    cexperttask.taskmember = factory.MemberName(expertApplication.AccountId);
                    cexperttask.taskmemberid = expertApplication.AccountId.GetValueOrDefault();
                    cexperttask.CaseStatusname = factory.StatusName(expertApplication.CaseStatusId);
                }
                cexperttask.taskexpert = factory.MemberName(taskList.AccountId);
                cexperttask.taskexpertid = taskList.AccountId.GetValueOrDefault();
                cexperttask.caseid = taskList.CaseId;
                cexperttask.taskcontent = taskList.TaskDetail;


                cexperttask.taskdatestart = taskList.TaskStartDate;
                cexperttask.taskdateend = taskList.TaskEndDate;
                cexperttask.taskprice = taskList.PayFrom;
                cexperttask.WorkPlace = (taskList.WorkPlace.GetValueOrDefault() ? "在家工作" : "指定地點工作");
                if (!taskList.WorkPlace.GetValueOrDefault())
                {

                    cexperttask.Address = taskList.Address;
                }
                else
                {
                    cexperttask.Address = "無";
                }

                list2.Add(cexperttask);
            
        }
            return Json(list2);
        }

        public async Task<IActionResult> Taskfromotherno()
        {
            _memberID = GetMemberIDFromSession();//登入者我自己memberID
            if (_memberID == 0)
            {
                TempData["message"] = "請先登入";
                return RedirectToAction("Login", "Member");
            }

            CExperTaskFactory factory = new CExperTaskFactory(_context);
            CExpertTaskViewModel cexperttask;
            List<CExpertTaskViewModel> list3 = new List<CExpertTaskViewModel>();
            //taskfromotherno
            //CaseId:15=專家尚未確認
            var q3 = _context.TaskLists
     .Include(b => b.ExpertApplications)
     .Where(x => x.IsExpert == true && x.AccountId == _memberID && x.ExpertApplications.Any(y => y.CaseStatusId == 15))
     .OrderBy(x => x.TaskEndDate)
     .Select(x => new
     {
         TaskList = x,
         ExpertApplications = x.ExpertApplications
                              .Where(ea => ea.CaseStatusId == 15)
                              .Select(ea => new { ea.CaseStatusId, ea.AccountId })
     })
     .ToList();

            foreach (var item in q3)
            {
                cexperttask = new CExpertTaskViewModel();
                var taskList = item.TaskList;

                foreach (var expertApplication in item.ExpertApplications)
                {
                    cexperttask.taskmember = factory.MemberName(expertApplication.AccountId);
                    cexperttask.taskmemberid = expertApplication.AccountId.GetValueOrDefault();
                    cexperttask.CaseStatusname = factory.StatusName(expertApplication.CaseStatusId);
                }
                cexperttask.taskexpert = factory.MemberName(taskList.AccountId);
                cexperttask.taskexpertid = taskList.AccountId.GetValueOrDefault();
                cexperttask.caseid = taskList.CaseId;
                cexperttask.taskcontent = taskList.TaskDetail;



                cexperttask.taskdatestart = taskList.TaskStartDate;
                cexperttask.taskdateend = taskList.TaskEndDate;
                cexperttask.taskprice = taskList.PayFrom;
                cexperttask.WorkPlace = (taskList.WorkPlace.GetValueOrDefault() ? "在家工作" : "指定地點工作");
                if (!taskList.WorkPlace.GetValueOrDefault())
                {

                    cexperttask.Address = taskList.Address;
                }
                else
                {
                    cexperttask.Address = "無";
                }

                list3.Add(cexperttask);

            }
            return Json(list3);
        }

        public async Task<IActionResult> Taskingfromother()
        {
            _memberID = GetMemberIDFromSession();//登入者我自己memberID
            if (_memberID == 0)
            {
                TempData["message"] = "請先登入";
                return RedirectToAction("Login", "Member");
            }

            CExperTaskFactory factory = new CExperTaskFactory(_context);
            CExpertTaskViewModel cexperttask;
            List<CExpertTaskViewModel> list4 = new List<CExpertTaskViewModel>();
            var q4 = _context.TaskLists
       .Include(b => b.ExpertApplications)
       .Where(x => x.IsExpert == true && x.AccountId == _memberID && x.ExpertApplications.Any(y => y.CaseStatusId == 17))
       .OrderBy(x => x.TaskEndDate)
       .Select(x => new
       {
           TaskList = x,
           ExpertApplications = x.ExpertApplications
                                .Where(ea => ea.CaseStatusId == 17)
                                .Select(ea => new { ea.CaseStatusId, ea.AccountId })
       })
       .ToList();



            foreach (var item in q4)
            {
                cexperttask = new CExpertTaskViewModel();
                var taskList = item.TaskList;
                foreach (var expertApplication in item.ExpertApplications)
                {
                    cexperttask.taskmember = factory.MemberName(expertApplication.AccountId);
                    cexperttask.taskmemberid = expertApplication.AccountId.GetValueOrDefault();
                    cexperttask.CaseStatusname = factory.StatusName(expertApplication.CaseStatusId);
                }

                cexperttask.taskexpert = factory.MemberName(taskList.AccountId);
                cexperttask.taskexpertid = taskList.AccountId.GetValueOrDefault();
                cexperttask.caseid = taskList.CaseId;
                cexperttask.taskcontent = taskList.TaskDetail;


                cexperttask.taskdatestart = taskList.TaskStartDate;
                cexperttask.taskdateend = taskList.TaskEndDate;
                cexperttask.taskprice = taskList.PayFrom;
                cexperttask.WorkPlace = (taskList.WorkPlace.GetValueOrDefault() ? "在家工作" : "指定地點工作");
                if (!taskList.WorkPlace.GetValueOrDefault())
                {

                    cexperttask.Address = taskList.Address;
                }
                else
                {
                    cexperttask.Address = "無";
                }

                list4.Add(cexperttask);
            }
            return Json(list4);
        }

        public async Task<IActionResult> Taskedfromother()
        {
            _memberID = GetMemberIDFromSession();//登入者我自己memberID
            if (_memberID == 0)
            {
                TempData["message"] = "請先登入";
                return RedirectToAction("Login", "Member");
            }

            CExperTaskFactory factory = new CExperTaskFactory(_context);
            CExpertTaskViewModel cexperttask;
            List<CExpertTaskViewModel> list5= new List<CExpertTaskViewModel>();
            //taskedfromother
            //CaseId:16=專家拒絕接案，18=專家案件已完成 13=委託人放棄委託//12專家案件已評價//11專家放棄委託


            var q5 = _context.TaskLists
    .Include(b => b.ExpertApplications)
    .Where(x => x.IsExpert == true && x.AccountId == _memberID && x.ExpertApplications.Any(y => y.CaseStatusId == 16 || y.CaseStatusId == 18 || y.CaseStatusId == 13 || y.CaseStatusId == 12 || y.CaseStatusId == 11))
    .OrderBy(x => x.TaskEndDate)
    .Select(x => new
    {
        TaskList = x,
        ExpertApplications = x.ExpertApplications
                             .Where(ea => ea.CaseStatusId == 16 || ea.CaseStatusId == 18 || ea.CaseStatusId == 13 || ea.CaseStatusId == 12 || ea.CaseStatusId == 11)
                             .Select(ea => new { ea.CaseStatusId, ea.AccountId })
    })
    .ToList();

            foreach (var item in q5)
            {
                cexperttask = new CExpertTaskViewModel();
                var taskList = item.TaskList;

                foreach (var expertApplication in item.ExpertApplications)
                {
                    cexperttask.taskmember = factory.MemberName(expertApplication.AccountId);
                    cexperttask.taskmemberid = expertApplication.AccountId.GetValueOrDefault();
                    cexperttask.CaseStatusname = factory.StatusName(expertApplication.CaseStatusId);
                }
                cexperttask.taskexpert = factory.MemberName(taskList.AccountId);
                cexperttask.taskexpertid = taskList.AccountId.GetValueOrDefault();
                cexperttask.caseid = taskList.CaseId;
                cexperttask.taskcontent = taskList.TaskDetail;


                cexperttask.taskdatestart = taskList.TaskStartDate;
                cexperttask.taskdateend = taskList.TaskEndDate;
                cexperttask.taskprice = taskList.PayFrom;
                cexperttask.WorkPlace = (taskList.WorkPlace.GetValueOrDefault() ? "在家工作" : "指定地點工作");
                if (!taskList.WorkPlace.GetValueOrDefault())
                {

                    cexperttask.Address = taskList.Address;
                }
                else
                {
                    cexperttask.Address = "無";
                }

                list5.Add(cexperttask);
            }
            return Json(list5);
        }

    }

}

    

 
