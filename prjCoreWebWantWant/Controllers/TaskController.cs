using Azure;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjCoreWantMember.ViewModels;
using prjCoreWebWantWant.Models;


using System.Text.Json;
using X.PagedList;

namespace prjWantWant_yh_CoreMVC.Controllers
{
    public class TaskController : Controller
    {
        private readonly NewIspanProjectContext _context;

        private readonly IWebHostEnvironment _host;
        public TaskController(NewIspanProjectContext context, IWebHostEnvironment host)
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

        public IActionResult List(CKeywordViewModel vm)
        {
            IEnumerable<TaskList> datas = null;
            if (string.IsNullOrEmpty(vm.txtKeyword))
            {
                datas = _context.TaskLists
                        .Include(t => t.Town.City)
                        .Where(tl => tl.PublishOrNot == "立刻上架");
            }
            else
            {
                datas = _context.TaskLists.Where(t => t.TaskTitle.ToUpper().Contains(vm.txtKeyword.ToUpper()) && t.PublishOrNot == "立刻上架" ||
                 t.TaskDetail.ToUpper().Contains(vm.txtKeyword.ToUpper()) && t.PublishOrNot == "立刻上架" ||
                 t.Address.ToUpper().Contains(vm.txtKeyword.ToUpper()) && t.PublishOrNot == "立刻上架"
                );
            }
            return View(datas);

            #region 匿名型別
            //using (NewIspanProjectContext db = new NewIspanProjectContext())              //匿名型別
            //{ 
            //var tasks = (from t in db.TaskLists
            //            where t.PublishOrNot == "立刻上架" /*&& DateTime.Parse(t.TaskStart) > DateTime.Now.Date*/
            //                select new
            //                {
            //                    t.TaskTitle,
            //                    t.TownId,
            //                    t.PayFrom,
            //                    t.RequiredNum,
            //                }).ToList();
            //    List<CTask> tasksList = new List<CTask>();
            //    foreach (var task in tasks)
            //    {
            //        CTask cTask = new CTask();

            //        cTask.TaskTitle = task.TaskTitle;

            //        cTask.TownId = (int)task.TownId;
            //        cTask.PayFrom = (int)task.PayFrom;
            //        tasksList.Add(cTask);
            //    }
            //    return View(tasksList);
            #endregion
        }

        public IActionResult Detail(int? id)
        {
            //var q = _context.TaskLists.Include(t => t.Town.City).Where(t => t.CaseId == id).FirstOrDefault();
            var q = _context.TaskLists
            .Include(t => t.Town.City)
            .Include(x => x.TaskSkills) 
            .Include(x => x.TaskCertificates) 
            .FirstOrDefault(t => t.CaseId == id);

            bool isAddressEmpty = string.IsNullOrEmpty(q.Address);
            ViewBag.IsAddressEmpty = isAddressEmpty;
            if (!string.IsNullOrEmpty(q.Address))
            {
                ViewBag.MapAddress = q.Address;
            }

        
            return View(q);
        }

        public IActionResult SortByUpdateTimeNew(string StartDate, string EndDate, bool workAtHome, string sortType, string city, string Category, CKeywordViewModel vm, int page = 1) //新到舊
        {
            var q = _context.TaskLists
                    .Include(t => t.Town.City)
                    .Include(t => t.TaskName)
                    .Where(tl => tl.PublishOrNot == "立刻上架" && tl.IsExpert != true);

            ViewBag.Category = Category;
            if (city != "請選擇地點" && city != "在家兼職")
            {
                q = q.Where(c => c.Town.City.City1 == city);
            }
            else if (city == "在家兼職")
            {
                q = q.Where(c => c.WorkPlace == true);
            }

            if (Category == "所有任務")
            {
                if (!string.IsNullOrEmpty(vm.txtKeyword))
                    q = q.Where(t => t.TaskTitle.Contains(vm.txtKeyword) || t.TaskDetail.Contains(vm.txtKeyword));
            }
            else
            {
                q = q.Where(t => t.TaskName.TaskName == Category).OrderByDescending(item => item.OnTop > DateTime.Now);
                if (!string.IsNullOrEmpty(vm.txtKeyword))
                    q = q.Where(t => t.TaskTitle.Contains(vm.txtKeyword) || t.TaskDetail.Contains(vm.txtKeyword));
            }        

            q = q.OrderByDescending(item => item.OnTop > DateTime.Now);
            q = q.OrderByDescending(t => t.DataModifyDate);

            IEnumerable<TaskList> enumerableQ = q.AsEnumerable();

            if (StartDate != null)
            {
                enumerableQ = enumerableQ.Where(c => DateTime.Parse(c.DataModifyDate) >= DateTime.Parse(StartDate));
                if (EndDate != null)
                {
                    enumerableQ = enumerableQ.Where(c => DateTime.Parse(c.DataModifyDate) <= DateTime.Parse(EndDate));
                }
            }

            //page = page < 1 ? 1 : page;
            //IEnumerable<TaskList> result = enumerableQ.ToPagedList(page, 6);
            return PartialView("Partial1", enumerableQ);
        }

        public IActionResult SortByUpdateTimeOld(string StartDate, string EndDate, bool workAtHome, string sortType, string city, string Category, CKeywordViewModel vm, int page = 1) //舊到新
        {
            var q = _context.TaskLists
                    .Include(t => t.Town.City)
                    .Include(t => t.TaskName)
                    .Where(tl => tl.PublishOrNot == "立刻上架" && tl.IsExpert != true);

            ViewBag.Category = Category;
            if (city != "請選擇地點" && city != "在家兼職")
            {
                q = q.Where(c => c.Town.City.City1 == city);
            }
            else if (city == "在家兼職")
            {
                q = q.Where(c => c.WorkPlace == true);
            }

            if (Category == "所有任務")
            {
                if (!string.IsNullOrEmpty(vm.txtKeyword))
                    q = q.Where(t => t.TaskTitle.Contains(vm.txtKeyword) || t.TaskDetail.Contains(vm.txtKeyword));
            }
            else
            {
                q = q.Where(t => t.TaskName.TaskName == Category).OrderByDescending(item => item.OnTop > DateTime.Now);
                if (!string.IsNullOrEmpty(vm.txtKeyword))
                    q = q.Where(t => t.TaskTitle.Contains(vm.txtKeyword) || t.TaskDetail.Contains(vm.txtKeyword));
            }

            q = q.OrderByDescending(item => item.OnTop > DateTime.Now);

            q = q.OrderBy(t => t.DataModifyDate);

            IEnumerable<TaskList> enumerableQ = q.AsEnumerable();

            if (StartDate != null)
            {
                enumerableQ = enumerableQ.Where(c => DateTime.Parse(c.DataModifyDate) >= DateTime.Parse(StartDate));
                if (EndDate != null)
                {
                    enumerableQ = enumerableQ.Where(c => DateTime.Parse(c.DataModifyDate) <= DateTime.Parse(EndDate));
                }
            }

            //page = page < 1 ? 1 : page;
            //IEnumerable<TaskList> result = enumerableQ.ToPagedList(page, 6);
            return PartialView("Partial1", enumerableQ);
        }

        public IActionResult SortBySalaryHigh(string StartDate, string EndDate, bool workAtHome, string sortType, string city, string Category, CKeywordViewModel vm, int page = 1) //高到低
        {
            var q = _context.TaskLists
                    .Include(t => t.Town.City)
                    .Include(t => t.TaskName)
                    .Where(tl => tl.PublishOrNot == "立刻上架" && tl.IsExpert != true);

            ViewBag.Category = Category;
            if (city != "請選擇地點" && city != "在家兼職")
            {
                q = q.Where(c => c.Town.City.City1 == city);
            }
            else if (city == "在家兼職")
            {
                q = q.Where(c => c.WorkPlace == true);
            }

            if (Category == "所有任務")
            {
                if (!string.IsNullOrEmpty(vm.txtKeyword))
                    q = q.Where(t => t.TaskTitle.Contains(vm.txtKeyword) || t.TaskDetail.Contains(vm.txtKeyword));
            }
            else
            {
                q = q.Where(t => t.TaskName.TaskName == Category).OrderByDescending(item => item.OnTop > DateTime.Now);
                if (!string.IsNullOrEmpty(vm.txtKeyword))
                    q = q.Where(t => t.TaskTitle.Contains(vm.txtKeyword) || t.TaskDetail.Contains(vm.txtKeyword));
            }

            q = q.OrderByDescending(item => item.OnTop > DateTime.Now);
            q = q.OrderByDescending(s => s.PayFrom);

            IEnumerable<TaskList> enumerableQ = q.AsEnumerable();

            if (StartDate != null)
            {
                enumerableQ = enumerableQ.Where(c => DateTime.Parse(c.DataModifyDate) >= DateTime.Parse(StartDate));
                if (EndDate != null)
                {
                    enumerableQ = enumerableQ.Where(c => DateTime.Parse(c.DataModifyDate) <= DateTime.Parse(EndDate));
                }
            }

            //page = page < 1 ? 1 : page;
            //IEnumerable<TaskList> result = enumerableQ.ToPagedList(page, 6);
            return PartialView("Partial1", enumerableQ);
        }

        public IActionResult SortBySalaryLow(string StartDate, string EndDate, bool workAtHome, string sortType, string city, string Category, CKeywordViewModel vm, int page = 1) //低到高
        {
            var q = _context.TaskLists
                    .Include(t => t.Town.City)
                    .Include(t => t.TaskName)
                    .Where(tl => tl.PublishOrNot == "立刻上架" && tl.IsExpert != true);

            ViewBag.Category = Category;
            if (city != "請選擇地點" && city != "在家兼職")
            {
                q = q.Where(c => c.Town.City.City1 == city);
            }
            else if (city == "在家兼職")
            {
                q = q.Where(c => c.WorkPlace == true);
            }
          
            if (Category == "所有任務")
            {
                if (!string.IsNullOrEmpty(vm.txtKeyword))
                    q = q.Where(t => t.TaskTitle.Contains(vm.txtKeyword) || t.TaskDetail.Contains(vm.txtKeyword));
            }
            else
            {
                q = q.Where(t => t.TaskName.TaskName == Category).OrderByDescending(item => item.OnTop > DateTime.Now);
                if (!string.IsNullOrEmpty(vm.txtKeyword))
                    q = q.Where(t => t.TaskTitle.Contains(vm.txtKeyword) || t.TaskDetail.Contains(vm.txtKeyword));
            }

            q = q.OrderByDescending(item => item.OnTop > DateTime.Now);

            q = q.OrderBy(s => s.PayFrom);

            IEnumerable<TaskList> enumerableQ = q.AsEnumerable();

            if (StartDate != null)
            {
                enumerableQ = enumerableQ.Where(c => DateTime.Parse(c.DataModifyDate) >= DateTime.Parse(StartDate));
                if (EndDate != null)
                {
                    enumerableQ = enumerableQ.Where(c => DateTime.Parse(c.DataModifyDate) <= DateTime.Parse(EndDate));
                }
            }

            //page = page < 1 ? 1 : page;
            //IEnumerable<TaskList> result = enumerableQ.ToPagedList(page, 6);
            return PartialView("Partial1", enumerableQ);
        }

        public IActionResult ListNew(CKeywordViewModel vm, int page = 1)
        {
            IEnumerable<TaskList> datas = null;
            

            if (string.IsNullOrEmpty(vm.txtKeyword))
            {
                datas = _context.TaskLists
                        .Include(t => t.Town.City)
                        .Include (t => t.TaskName)
                        //.Include(x => x.TaskSkills)
                        .Where(tl => tl.PublishOrNot == "立刻上架" && tl.IsExpert != true).OrderByDescending(item => item.OnTop > DateTime.Now);
                //var paginatedData = datas.Skip((page - 1) * pageSize).Take(pageSize);
                //return View(paginatedData);
            }
            else
            {
                datas = _context.TaskLists.Where(t => t.TaskTitle.ToUpper().Contains(vm.txtKeyword.ToUpper()) && t.PublishOrNot == "立刻上架" ||
                 t.TaskDetail.ToUpper().Contains(vm.txtKeyword.ToUpper()) && t.PublishOrNot == "立刻上架" ||
                 t.Address.ToUpper().Contains(vm.txtKeyword.ToUpper()) && t.PublishOrNot == "立刻上架"
                );
            }

            //if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            //    datas.Where(t => t.AccountId != GetAccountID());
            
            ViewBag.TotalCount = datas.Count();
            page = page < 1 ? 1 : page;
            datas = datas.ToPagedList(page, 6);


            return View(datas);
        }

        public IActionResult Partial1(string StartDate, string EndDate, string sortType, string city, string Category, CKeywordViewModel vm, int page = 1)
        {
            var q = _context.TaskLists
                    .Include(t => t.Town.City)
                    .Include(t => t.TaskName)
                    .Where(tl => tl.PublishOrNot == "立刻上架"&& tl.IsExpert != true);

            ViewBag.Category = Category;
            if (city != "請選擇地點" && city != "在家兼職")
            {
                q = q.Where(c => c.Town.City.City1 == city);  //TODO 會有例外狀況
            }
            else if(city == "在家兼職")
            {
                q = q.Where(c => c.WorkPlace == true);
            }

            if (Category == "所有任務")
            {
                if (!string.IsNullOrEmpty(vm.txtKeyword))
                    q = q.Where(t => t.TaskTitle.Contains(vm.txtKeyword) || t.TaskDetail.Contains(vm.txtKeyword));
            }
            else
            {
                q = q.Where(t => t.TaskName.TaskName == Category).OrderByDescending(item => item.OnTop > DateTime.Now);
                if (!string.IsNullOrEmpty(vm.txtKeyword))
                    q = q.Where(t => t.TaskTitle.Contains(vm.txtKeyword) || t.TaskDetail.Contains(vm.txtKeyword));
            }

            q = q.OrderByDescending(item => item.OnTop > DateTime.Now);

            IEnumerable<TaskList> enumerableQ = q.AsEnumerable();

            if (StartDate != null)
            {
                enumerableQ = enumerableQ.Where(c => DateTime.Parse(c.DataModifyDate) >= DateTime.Parse(StartDate));
                if (EndDate != null)
                {
                    enumerableQ = enumerableQ.Where(c => DateTime.Parse(c.DataModifyDate) <= DateTime.Parse(EndDate));
                }
            }

            //page = page < 1 ? 1 : page;
            //IEnumerable<TaskList> result = enumerableQ.ToPagedList(page, 6);
            return PartialView(enumerableQ);
        }

        public IActionResult Count(string StartDate, string EndDate, string sortType, string city, string Category, CKeywordViewModel vm, int page = 1)
        {
            var q = _context.TaskLists
                    .Include(t => t.Town.City)
                    .Include(t => t.TaskName)
                    .Where(tl => tl.PublishOrNot == "立刻上架" && tl.IsExpert != true);

            ViewBag.Category = Category;
            if (city != "請選擇地點" && city != "在家兼職")
            {
                q = q.Where(c => c.Town.City.City1 == city);  //TODO 會有例外狀況
            }
            else if (city == "在家兼職")
            {
                q = q.Where(c => c.WorkPlace == true);
            }

            if (Category == "所有任務")
            {
                if (!string.IsNullOrEmpty(vm.txtKeyword))
                    q = q.Where(t => t.TaskTitle.Contains(vm.txtKeyword) || t.TaskDetail.Contains(vm.txtKeyword));
            }
            else
            {
                q = q.Where(t => t.TaskName.TaskName == Category).OrderByDescending(item => item.OnTop > DateTime.Now);
                if (!string.IsNullOrEmpty(vm.txtKeyword))
                    q = q.Where(t => t.TaskTitle.Contains(vm.txtKeyword) || t.TaskDetail.Contains(vm.txtKeyword));
            }

            q = q.OrderByDescending(item => item.OnTop > DateTime.Now);

            IEnumerable<TaskList> enumerableQ = q.AsEnumerable();

            if (StartDate != null)
            {
                enumerableQ = enumerableQ.Where(c => DateTime.Parse(c.DataModifyDate) >= DateTime.Parse(StartDate));
                if (EndDate != null)
                {
                    enumerableQ = enumerableQ.Where(c => DateTime.Parse(c.DataModifyDate) <= DateTime.Parse(EndDate));
                }
            }

            int Count = enumerableQ.Distinct().Count();
            return Json(Count);
        }

        public IActionResult Apply(int? resumeId, int? caseId)
        {
            ApplicationList applicationList = new ApplicationList()
            {
                ResumeId = resumeId,        //todo 還沒抓到履歷id(多履歷的問題)
                CaseId = caseId,
                CaseStatusId = 21,
                ApplicationDate = DateTime.Now.Date.ToString("yyyy-MM-dd"),
            };
            _context.Add(applicationList);
            _context.SaveChanges(true);
            return RedirectToAction("ListNew");
        }
    }
}
