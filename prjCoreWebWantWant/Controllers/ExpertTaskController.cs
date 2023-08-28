using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjCoreWebWantWant.Models;
using prjCoreWebWantWant.ViewModels;

namespace prjCoreWebWantWant.Controllers
{
    public class ExpertTaskController : Controller
    {
        private readonly NewIspanProjectContext _context;
        
        private int _memberID;

        public ExpertTaskController(NewIspanProjectContext context)
        {
            _context = context;
            
           

        }

      

        #region 歷史委託
        // GET: ExpertTask
        //public async Task<IActionResult> ExpertList() //歷史委託
        //{
        //    var newIspanProjectContext = _context.TaskLists.Include(t => t.Payment).Include(t => t.PaymentDate).Include(t => t.Salary).Include(t => t.Town);
        //    return View(await newIspanProjectContext.ToListAsync());
        //}

        public IActionResult ExpertListnew() //歷史委託
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
            //mytasking
            //CaseId:15=專家尚未確認，17=專家案件進行中
            var q1 = _context.TaskLists
     .Include(b => b.ExpertApplications)
     .Where(x => x.IsExpert == true && x.ExpertApplications.Any(y => y.AccountId == _memberID && (y.CaseStatusId == 15 || y.CaseStatusId == 17)))
     .OrderBy(x => x.TaskEndDate)
     .Select(x => new
     {
         TaskList = x,
         ExpertApplications = x.ExpertApplications
                              .Where(ea => ea.AccountId == _memberID && (ea.CaseStatusId == 15 || ea.CaseStatusId == 17))
                              .Select(ea => new { ea.CaseStatusId ,ea.AccountId})
     })
     .ToList();
            foreach (var item in q1)
            {
                cexperttask = new CExpertTaskViewModel();
                var taskList = item.TaskList;
                foreach (var expertApplication in item.ExpertApplications)
                {
                    cexperttask.taskmember = factory.MemberName(expertApplication.AccountId);
                    cexperttask.CaseStatusname = factory.StatusName(expertApplication.CaseStatusId);
                    // 使用上述的變數進行其他操作
                }
               
                cexperttask.taskexpert = factory.MemberName(taskList.AccountId);
                cexperttask.caseid = taskList.CaseId;
                if (taskList.TaskDetail.Length>20) {
                    cexperttask.taskcontent = taskList.TaskDetail.Substring(0, 20) + "...";
                }
                else
                {
                    cexperttask.taskcontent = taskList.TaskDetail;
                }
                    
                cexperttask.taskdatestart = taskList.TaskStartDate;
                cexperttask.taskdateend = taskList.TaskEndDate;
                cexperttask.taskprice = taskList.PayFrom;
                
                list.Add(cexperttask);
            }
            vw.mytasking=list;
            //下面要重改QQQQQQQQ
            //===============================================================
            
            List<CExpertTaskViewModel> list2 = new List<CExpertTaskViewModel>();
            //mytasked
            //CaseId:16=專家拒絕接案，18=專家案件已完成
            var q2 = _context.TaskLists
                .Include(b => b.ExpertApplications)
                .Where(x => x.IsExpert == true && (x.CaseStatusId == 16|| x.CaseStatusId == 18) && x.ExpertApplications.Any(y => y.AccountId == _memberID))
                .OrderBy(x => x.TaskEndDate)
                .ToList();
            foreach (var item in q2)
            {
                cexperttask = new CExpertTaskViewModel();
                foreach (var application in item.ExpertApplications)
                {
                    cexperttask.taskmember = factory.MemberName(application.AccountId);
                }
                cexperttask.taskexpert = factory.MemberName(item.AccountId);
                cexperttask.caseid = item.CaseId;
                if (item.TaskDetail.Length > 20)
                {
                    cexperttask.taskcontent = item.TaskDetail.Substring(0, 20)+"...";
                }
                else
                {
                    cexperttask.taskcontent = item.TaskDetail;
                }

                cexperttask.taskdatestart = item.TaskStartDate;
                cexperttask.taskdateend = item.TaskEndDate;
                cexperttask.taskprice = item.PayFrom;
                cexperttask.CaseStatusname = factory.StatusName(item.CaseStatusId);
                list2.Add(cexperttask);
            }
            vw.mytasked = list2;
            //===============================================================
            List<CExpertTaskViewModel> list3 = new List<CExpertTaskViewModel>();
            //taskfromotherno
            //CaseId:15=專家尚未確認
            var q3 = _context.TaskLists
                .Include(b => b.ExpertApplications)
                .Where(x => x.IsExpert == true && x.CaseStatusId == 15  && x.AccountId==_memberID)
                .OrderBy(x => x.TaskEndDate)
                .ToList();
            foreach (var item in q3)
            {
                cexperttask = new CExpertTaskViewModel();
                foreach (var application in item.ExpertApplications)
                {
                    cexperttask.taskmember = factory.MemberName(application.AccountId);
                }
                cexperttask.taskexpert = factory.MemberName(item.AccountId);
                cexperttask.caseid = item.CaseId;
                if (item.TaskDetail.Length > 20)
                {
                    cexperttask.taskcontent = item.TaskDetail.Substring(0, 20) + "...";
                }
                else
                {
                    cexperttask.taskcontent = item.TaskDetail;
                }

                cexperttask.taskdatestart = item.TaskStartDate;
                cexperttask.taskdateend = item.TaskEndDate;
                cexperttask.taskprice = item.PayFrom;
                cexperttask.CaseStatusname = factory.StatusName(item.CaseStatusId);
                list3.Add(cexperttask);
            }
            vw.taskfromotherno = list3;
            //===============================================================
           
            List<CExpertTaskViewModel> list4 = new List<CExpertTaskViewModel>();
            //taskingfromother
            //CaseId:17=專家案件進行中
            var q4 = _context.TaskLists
                .Include(b => b.ExpertApplications)
                .Where(x => x.IsExpert == true && x.CaseStatusId == 17 && x.AccountId == _memberID)
                .OrderBy(x => x.TaskEndDate)
                .ToList();
            foreach (var item in q4)
            {
                cexperttask = new CExpertTaskViewModel();
                foreach (var application in item.ExpertApplications)
                {
                    cexperttask.taskmember = factory.MemberName(application.AccountId);
                }
                cexperttask.taskexpert = factory.MemberName(item.AccountId);
                cexperttask.caseid = item.CaseId;
                if (item.TaskDetail.Length > 20)
                {
                    cexperttask.taskcontent = item.TaskDetail.Substring(0, 20) + "...";
                }
                else
                {
                    cexperttask.taskcontent = item.TaskDetail;
                }

                cexperttask.taskdatestart = item.TaskStartDate;
                cexperttask.taskdateend = item.TaskEndDate;
                cexperttask.taskprice = item.PayFrom;
                cexperttask.CaseStatusname = factory.StatusName(item.CaseStatusId);
                list4.Add(cexperttask);
            }
            vw.taskingfromother = list4;
            //===============================================================
            List<CExpertTaskViewModel> list5 = new List<CExpertTaskViewModel>();
            //taskedfromother
            //CaseId:16=專家拒絕接案，18=專家案件已完成
            var q5 = _context.TaskLists
                .Include(b => b.ExpertApplications)
                .Where(x => x.IsExpert == true && (x.CaseStatusId == 16|| x.CaseStatusId==18) && x.AccountId == _memberID)
                .OrderBy(x => x.TaskEndDate)
                .ToList();
            foreach (var item in q5)
            {
                cexperttask = new CExpertTaskViewModel();
                foreach (var application in item.ExpertApplications)
                {
                    cexperttask.taskmember = factory.MemberName(application.AccountId);
                }
                cexperttask.taskexpert = factory.MemberName(item.AccountId);
                cexperttask.caseid = item.CaseId;
                if (item.TaskDetail.Length > 20)
                {
                    cexperttask.taskcontent = item.TaskDetail.Substring(0, 20) + "...";
                }
                else
                {
                    cexperttask.taskcontent = item.TaskDetail;
                }

                cexperttask.taskdatestart = item.TaskStartDate;
                cexperttask.taskdateend = item.TaskEndDate;
                cexperttask.taskprice = item.PayFrom;
                cexperttask.CaseStatusname = factory.StatusName(item.CaseStatusId);
                list5.Add(cexperttask);
            }
            vw.taskedfromother = list5;
            return View(vw);
            
        }

        #endregion

        // GET: ExpertTask/Details/5
        public async Task<IActionResult> DetailsReceive(int? id)
        {
            if (id == null || _context.TaskLists == null)
            {
                //return NotFound();
                return View();            }

            var taskList = await _context.TaskLists
                .Include(t => t.Payment)
                .Include(t => t.PaymentDate)
                .Include(t => t.Salary)
                .Include(t => t.Town)
                .FirstOrDefaultAsync(m => m.CaseId == id);
            if (taskList == null)
            {
                //return NotFound();
                return View();            }

            return View(taskList);
        }

# region 收到委託
        public async Task<IActionResult> DetailsResponse(int? id) //收到委託
        {
            if (id == null || _context.TaskLists == null)
            {
                //return NotFound();
                return View();            }

            var taskList = await _context.TaskLists
                .Include(t => t.Payment)
                .Include(t => t.PaymentDate)
                .Include(t => t.Salary)
                .Include(t => t.Town)
                .FirstOrDefaultAsync(m => m.CaseId == id);
            if (taskList == null)
            {
                //return NotFound();
                return View();
            }

            return View(taskList);
        }
#endregion

        #region 檢視
        public async Task<IActionResult> DetailsView(int? id)//檢視
        {
            if (id == null || _context.TaskLists == null)
            {
                //return NotFound();
                return View();
            }

            var taskList = await _context.TaskLists
                .Include(t => t.Payment)
                .Include(t => t.PaymentDate)
                .Include(t => t.Salary)
                .Include(t => t.Town)
                .FirstOrDefaultAsync(m => m.CaseId == id);
            if (taskList == null)
            {
                //return NotFound();
                return View();
            }

            return View(taskList);
        }

        #endregion
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

        #region 從履歷連委託單

        // GET: ExpertTask/Create
        public IActionResult Create(int expertaccountid)  //從履歷連委託單
        {
            _memberID = GetMemberIDFromSession();//登入者我自己memberID
            if (_memberID == 0)
            {
                TempData["message"] = "請先登入";
                return RedirectToAction("Login","Member");
            }


            CExperTaskFactory factory = new CExperTaskFactory(_context);
           string expertname= factory.MemberName(expertaccountid);
            string accountname = factory.MemberName(_memberID);
            CExpertTaskInsertViewModel vm = new CExpertTaskInsertViewModel();
            vm.委託人 = accountname;
            vm.委託人ID = _memberID;
            vm.被委託人 = expertname;
            vm.被委託人ID = expertaccountid;
            ViewBag.expertid = expertaccountid;
            return View(vm);
        }

        // POST: ExpertTask/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]  //從履歷連委託單
        public async Task<IActionResult> Create(CExpertTaskInsertViewModel vm)
        {
            CExperTaskFactory factory = new CExperTaskFactory(_context);
            if (ModelState.IsValid)
            {
                TaskList tasklist = new TaskList();
                tasklist.AccountId = factory.MemberID(vm.被委託人);
                tasklist.TaskTitle = vm.委託人 + "跟專家" + vm.被委託人 + "的委託案件";
                tasklist.TaskDetail = vm.委託內容;
                tasklist.PayFrom = vm.委託價格;
                tasklist.PayTo = vm.委託價格;
                if(vm.委託工作地點 == "在家工作")
                {
                    tasklist.WorkPlace = true;
                    tasklist.Address = "無";
                }
                else if (vm.委託工作地點 == "指定地點工作")
                {
                    tasklist.WorkPlace =false;
                    tasklist.Address = vm.指定委託地點;
                }

            
                    tasklist.TaskStartDate = vm.委託時間起;
                    tasklist.TaskEndDate = vm.委託時間訖;
               
                DateTime date= DateTime.Now;
                tasklist.DataCreateDate = date.ToString();
                //tasklist.CaseStatusId = 15;//
                tasklist.IsExpert = true;
                _context.TaskLists.Add(tasklist);
                await _context.SaveChangesAsync();
                int taskid = tasklist.CaseId;

                ExpertApplication ea=new ExpertApplication();
                ea.CaseId = taskid;
                ea.AccountId = factory.MemberID(vm.委託人);
                ea.CaseStatusId = 15;  //專家尚未確認
                ea.ExpertAccountId= factory.MemberID(vm.被委託人);
                _context.ExpertApplications.Add(ea);
                await _context.SaveChangesAsync();


                TempData["message"] = "委託已送出!請靜候專家回覆，或使用聊天室通知專家。";
               
                return RedirectToAction("ExpertMainPage", "Expert");
            }
            var errors = ModelState.Select(x => x.Value.Errors)
                        .Where(y => y.Count > 0)
                        .ToList();

            foreach(var item in errors)
            {
                vm.委託內容 += item;
            }
           
            return View(vm);
        }

        #endregion
        #region 修改委託單
        // GET: ExpertTask/Edit/5
        public async Task<IActionResult> Edit(int? id) //修改委託單
        {
            if (id == null || _context.TaskLists == null)
            {
                return NotFound();
                
            }

            var taskList = await _context.TaskLists.FindAsync(id);
            if (taskList == null)
            {
                return NotFound();
                
            }
            CExperTaskFactory factory = new CExperTaskFactory(_context);
            CExpertTaskInsertViewModel vm = new CExpertTaskInsertViewModel();
            ExpertApplication ea = _context.ExpertApplications
                .Where(x=>x.CaseId==id)
                .FirstOrDefault();
            vm.委託人 = factory.MemberName(ea.AccountId);
            vm.委託人ID = ea.AccountId.GetValueOrDefault();
            vm.被委託人 = factory.MemberName(taskList.AccountId);
            vm.被委託人ID = taskList.AccountId.GetValueOrDefault();
            vm.委託內容 = taskList.TaskDetail;
            vm.委託時間起 = taskList.TaskStartDate;
            vm.委託時間訖 = taskList.TaskEndDate;
            vm.委託價格 = taskList.PayFrom.GetValueOrDefault();
            
            if (taskList.WorkPlace == true)
            {
                vm.委託工作地點 = "在家工作";
                
            }
            else if (taskList.WorkPlace == false)
            {
                vm.委託工作地點 = "指定地點工作";
                vm.指定委託地點 = taskList.Address;
            }


            return View(taskList);
        }

        // POST: ExpertTask/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]  //修改委託單
        public async Task<IActionResult> Edit(int id, [Bind("CaseId,AccountId,TaskNameId,TaskTitle,TaskDetail,WorkingHoursId,PayFrom,PayTo,PaymentId,PaymentDateId,SalaryId,TaskPlace,TownId,WorkPlace,Address,RequiredNum,TaskPeriod,TaskStartHour,TaskEndHour,TaskStartDate,TaskEndDate,Requirement,HumanList,LanguageRequired,ServiceStatus,StatusChangeReasonId,PublishOrNot,PublishStart,PublishEnd,CaseStatusId,OnTop,DataCreateDate,DataModifyDate,DataModifyPerson,IsExpert")] TaskList taskList)
        {
            if (id != taskList.CaseId)
            {
                return NotFound();
               
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taskList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskListExists(taskList.CaseId))
                    {
                        //return NotFound();
                        return View();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ExpertListnew));
            }
            ViewData["PaymentId"] = new SelectList(_context.Payments, "PaymentId", "PaymentId", taskList.PaymentId);
            ViewData["PaymentDateId"] = new SelectList(_context.PaymentDates, "PaymentDateId", "PaymentDateId", taskList.PaymentDateId);
            ViewData["SalaryId"] = new SelectList(_context.Salaries, "SalaryId", "SalaryId", taskList.SalaryId);
            ViewData["TownId"] = new SelectList(_context.Towns, "TownId", "TownId", taskList.TownId);
            return View(taskList);
        }
        #endregion





        //-------------------------------------------------
        private bool TaskListExists(int id)
        {
          return (_context.TaskLists?.Any(e => e.CaseId == id)).GetValueOrDefault();
        }
    }
}
