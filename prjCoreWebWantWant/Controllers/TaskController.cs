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
       
        public IActionResult List(CKeywordViewModel vm)
        {
            NewIspanProjectContext db = new NewIspanProjectContext();
            //var town = from t in db.TaskLists
            //           where t.TownId == t.Town.TownId && t.Town.CityId == t.Town.City.CityId
            //           select t.Town.City.City1;
            //ViewBag.CITY = town.FirstOrDefault();

            IEnumerable<TaskList> datas = null;
            if (string.IsNullOrEmpty(vm.txtKeyword))
            {
                //datas = from t in db.TaskLists
                //        where t.PublishOrNot == "立刻上架"
                //        select t;  
                datas = db.TaskLists
                        .Include(t => t.Town.City)
                        .Where(tl => tl.PublishOrNot == "立刻上架");
            }
            else
            {
                //datas = db.TaskLists.Where(t => t.TaskTitle.ToUpper().Contains(vm.txtKeyword.ToUpper()));

                datas = db.TaskLists.Where(t => t.TaskTitle.ToUpper().Contains(vm.txtKeyword.ToUpper()) && t.PublishOrNot == "立刻上架" ||
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
            NewIspanProjectContext db = new NewIspanProjectContext();
            TaskList task = db.TaskLists.FirstOrDefault(p => p.CaseId == id);
            if (task == null)
                return RedirectToAction("List");
            CTaskWrap taskWrap = new CTaskWrap();
            taskWrap.task = task;
            return View(taskWrap);
        }
        [HttpPost]
        public ActionResult Detail(CTaskWrap pIn)
        {
            NewIspanProjectContext db = new NewIspanProjectContext();
            TaskList pDb = db.TaskLists.FirstOrDefault(p => p.CaseId == pIn.FId);
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


        public IActionResult Partial(string Category)
        {
            NewIspanProjectContext db = new NewIspanProjectContext();
            //var q = from t in db.TaskLists
            //        where t.TaskName.TaskName == Category && t.PublishOrNot == "立刻上架"
            //        select t;
            if (Category == "請選擇任務類型")
            {
                var all = db.TaskLists.
                    Include(t => t.Town.City).
                    Where(t => t.PublishOrNot == "立刻上架");
                return PartialView(all);
            }

            var q = db.TaskLists.
                     Include(t => t.Town.City).
                     Where(t => t.TaskName.TaskName == Category && t.PublishOrNot == "立刻上架");
            return PartialView(q);
        }
    }
}
