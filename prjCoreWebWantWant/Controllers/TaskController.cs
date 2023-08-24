using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjCoreWantMember.ViewModels;
using prjCoreWebWantWant.Models;


using System.Text.Json;

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
            if (id == null)
                return RedirectToAction("List");
            TaskList task = _context.TaskLists.FirstOrDefault(p => p.CaseId == id);
            if (task == null)
                return RedirectToAction("List");
            CTaskWrap taskWrap = new CTaskWrap();
            taskWrap.task = task;
            return View(taskWrap);
        }
        [HttpPost]
        public ActionResult Detail(CTaskWrap pIn)
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

        public IActionResult Sort()
        {
            return RedirectToAction("List");
        }

        public ActionResult Apply(int id)
        {
            // 根據id執行必要的操作，獲取數據等

            // 返回 apply.cshtml 視圖
            return View("Apply");
        }


        public IActionResult Partial(string Category,CKeywordViewModel vm)
        {
            var q = _context.TaskLists
                    .Include(t => t.Town.City)
                    .Where(tl => tl.PublishOrNot == "立刻上架");

            if (Category == "所有任務類型")
            {
                if(!string.IsNullOrEmpty(vm.txtKeyword))
                    q = q.Where(t => t.TaskTitle.Contains(vm.txtKeyword));
            }
            else
            {
                q = q.Where(t => t.TaskName.TaskName == Category);
                if (!string.IsNullOrEmpty(vm.txtKeyword))
                    q = q.Where(t => t.TaskTitle.Contains(vm.txtKeyword));
            }
            return PartialView(q);
        }

        public IActionResult ListNew(CKeywordViewModel vm)
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
        }

        public IActionResult Partial1(string Category, CKeywordViewModel vm)
        {
            var q = _context.TaskLists
                    .Include(t => t.Town.City)
                    .Where(tl => tl.PublishOrNot == "立刻上架");

            if (Category == "所有任務類型")
            {
                if (!string.IsNullOrEmpty(vm.txtKeyword))
                    q = q.Where(t => t.TaskTitle.Contains(vm.txtKeyword));
            }
            else
            {
                q = q.Where(t => t.TaskName.TaskName == Category);
                if (!string.IsNullOrEmpty(vm.txtKeyword))
                    q = q.Where(t => t.TaskTitle.Contains(vm.txtKeyword));
            }
            return PartialView(q);
        }
    }
}
