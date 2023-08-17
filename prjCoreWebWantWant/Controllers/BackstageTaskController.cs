
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using prjCoreWebWantWant.Models;
using WantTask.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WantTask.Controllers
{

    public class BackstageTaskController : Controller
    {

        private readonly NewIspanProjectContext _context;

        private readonly IWebHostEnvironment _host;
        public BackstageTaskController(NewIspanProjectContext context, IWebHostEnvironment host)
        {
            _context = context;
            _host = host;
        }

        public IActionResult PartialPublish(string category)
        {
            var taskName = _context.TaskLists.
                Where(t => t.TaskName.TaskName == category && t.PublishOrNot == "立刻上架");
            //var cities = _context.Address.Select(c => new
            //{
            //    c.City
            //}).Distinct();
            return PartialView(taskName);
        }

        //選擇任務類別+未上架
        public IActionResult PartialNoPublish(string category)
        {
            var taskName = _context.TaskLists.
                Where(t => t.TaskName.TaskName == category && t.PublishOrNot == "延後上架");
            //var cities = _context.Address.Select(c => new
            //{
            //    c.City
            //}).Distinct();
            return PartialView(taskName);
        }


        public IActionResult Approve()
        {
                 var query =( from app in _context.ApplicationLists
                             join task in _context.TaskLists on app.CaseId equals task.CaseId
                             join resume in _context.Resumes on app.ResumeId equals resume.ResumeId
                             join member in _context.MemberAccounts on resume.AccountId equals member.AccountId
                             join resumeskill in _context.ResumeSkills on resume.ResumeId equals resumeskill.ResumeId
                             join skill in _context.Skills on resumeskill.SkillId equals skill.SkillId
                             join resumecer in _context.ResumeCertificates on resume.ResumeId equals resumecer.ResumeId
                             join cer in _context.Certificates on resumecer.CertificateId equals cer.CertificateId
                           
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
                                 TaskStart = task.TaskStartDate,
                                 TaskTitle = task.TaskTitle,
                                 TaskDetail = task.TaskDetail
                             }).Distinct();
          

            var viewModelList = query.ToList();

            return View(viewModelList);

        }


        public IActionResult Accept(int? id)
        {
            ApplicationList task = _context.ApplicationLists.FirstOrDefault(p => p.CaseId == id);
            if (id != null)
            {
                task.CaseStatusId = 1;
                _context.SaveChanges();
            }
            return RedirectToAction("Approve");
        }
       
        public IActionResult Refuse(int? id)
        {
            ApplicationList task = _context.ApplicationLists.FirstOrDefault(p => p.CaseId == id);
            if (id != null)
            {
                task.CaseStatusId = 2;
                _context.SaveChanges();
            }
            return RedirectToAction("Approve");
        }

        //public IActionResult Form()
        //{
        //    return View("Form");
        //}

        //public IActionResult Form( CCreateTask createTask)
        //{  
            
            
        //    return View("Form");
        //}

        public IActionResult Form(TaskList taskList)
        {
            //test
            //if (string.IsNullOrEmpty(createTask.TaskTitle))
            //{
            //    createTask.TaskTitle = "guest";
            //}
            //return Content($"Hello {createTask.TaskTitle} , you are {createTask.TaskDetail} years old.");
            
            _context.TaskLists.Add(taskList);
            _context.SaveChanges();

            return View("Form");

            //return Content("新增成功!!");
        }

        //public IActionResult JobdetailBackstage()
        //{
        //    return View("JobdetailBackstage");
        //}

        public IActionResult TablesEditable()
        {            
            var q = from t in _context.TaskLists
                    join tt in _context.TaskNameLists on t.TaskNameId equals tt.TaskNameId
                          where t.PublishOrNot == "立刻上架" && t.TaskNameId == tt.TaskNameId
                          select t;
            return View(q);
        }



        //public IActionResult yes()
        //{
        //    var q = from app in _context.ApplicationLists.AsEnumerable()

        //            where app.TaskList.TaskNameID == (int)this.Tag && app.CaseStatusID == 21

        //            select app;
        //}

        public IActionResult JobdetailBackstage()
        { 
            return View(JobdetailBackstage);   
        }


        //public IActionResult JobdetailBackstage(int? id)     //修改
        //{
        //    if (id == null)
        //    {
        //        return RedirectToAction("JobdetailBackstage");
        //    }

        //    TaskList task = _context.TaskLists.FirstOrDefault(p => p.CaseId == id);
        //    if (task == null)
        //    {
        //        return RedirectToAction("JobdetailBackstage");
        //    }
        //    CTaskWrap taskWrap = new CTaskWrap();  //因要改成用class:CProductWrap，所以增加這兩行
        //    taskWrap.task = task;
        //    return View(taskWrap);   //原本括號內是prod
        //}

        //[HttpPost]

        //public IActionResult JobdetailBackstage(CTaskWrap pln)     //修改，原本括號內是用TProduct pln，改成用class:CProductWrap，因CProduct是自動產生，會產生多次，用Wrap就不會被影響
        //{

        //    TaskList pDb = _context.TaskLists.FirstOrDefault(p => p.CaseId == pln.FId);

        //    if (pDb != null)
        //    {
        //        //if (pln.photo != null)
        //        //{
        //        //    string photoName = Guid.NewGuid().ToString() + ".jpg";
        //        //    pDb.FImagePath = photoName;
        //        //    pln.photo.CopyTo(new FileStream(_enviro.WebRootPath + "/Images/" + photoName, FileMode.Create));
        //        //    //原本加照片的方法改成copyto，filestream裡面有兩個參數，1.路徑+存到哪個資料夾+給檔名，2.如果沒有照片就create

        //        //}
        //        pDb.TaskTitle = pln.FTitle;
        //        pDb.TaskDetail = pln.FDetail;
        //        pDb.PayFrom = pln.FPayFrom;
        //        //pDb.FPrice = pln.FPrice;
        //        _context.SaveChanges();
        //    }
        //    return RedirectToAction("JobdetailBackstage");
        //}




        public IActionResult TaskName()
        {
            var taskName = _context.TaskNameLists.Select(c => c.TaskName).Distinct();
            //var cities = _context.Address.Select(c => new
            //{
            //    c.City
            //}).Distinct();
            return Json(taskName);
        }

        //public IActionResult TablesEditable(TaskList taskList)
        //{
        //    NewIspanProjectContext _context = new NewIspanProjectContext();

        //    if (taskList != null && taskList.PublishOrNot != null)
        //    {
        //        var tasklist = _context.TaskList.Where(t => t.PublishOrNot == "立刻上架").ToList();
        //        return View(tasklist);
        //    }

        //    return Content("No tasks found.");

        //}

        //支薪方式
        public IActionResult Payment()
        {
            var payment = _context.Payments.Select(c => c.Payment1).Distinct();
            //var cities = _context.Address.Select(c => new
            //{
            //    c.City
            //}).Distinct();
            return Json(payment);
        }


        //支薪日
        public IActionResult PaymentDate()
        {
            var paymentDate = _context.PaymentDates.Select(c => c.PaymentDate1).Distinct();
            //var cities = _context.Address.Select(c => new
            //{
            //    c.City
            //}).Distinct();
            return Json(paymentDate);
        }



        public IActionResult Cities()
        {
            var cities = _context.Cities.Select(c => c.City1).Distinct();
            //var cities = _context.Address.Select(c => new
            //{
            //    c.City
            //}).Distinct();
            return Json(cities);
        }

        //根據城市名稱，回傳城市的鄉鎮區JSON資料
        public IActionResult Districts(string city)
        {
            var districts = _context.Cities
                .Where(a => a.City1 == city)
                .SelectMany(c => c.Towns)
                .Select(c => c.Town1)
                .Distinct();

            return Json(districts);
        }

        //技能種類
        public IActionResult SkillBig()
        {
            var skillBig = _context.SkillTypes.Select(c => c.SkillTypeName).Distinct();
            //var cities = _context.Address.Select(c => new
            //{
            //    c.City
            //}).Distinct();
            return Json(skillBig);
        }

        //技能小項目
        public IActionResult SkillSmall(string skillBig)
        {
            var skillSmall = _context.SkillTypes
                .Where(a => a.SkillTypeName == skillBig)
                .SelectMany(c => c.Skills)
                .Select(c => c.SkillName)
                .Distinct();

            return Json(skillSmall);
        }


        //證照種類
        public IActionResult CerBig()
        {
            var cerBig = _context.CertificateTypes.Select(c => c.CertificateTypeName).Distinct();
            //var cities = _context.Address.Select(c => new
            //{
            //    c.City
            //}).Distinct();
            return Json(cerBig);
        }

        //證照小項目
        public IActionResult CerSmall(string cerBig)
        {
            var cerSmall = _context.CertificateTypes
                .Where(a => a.CertificateTypeName == cerBig)
                .SelectMany(c => c.Certificates)
                .Select(c => c.CertificateName)
                .Distinct();

            return Json(cerSmall);
        }

    }
}
