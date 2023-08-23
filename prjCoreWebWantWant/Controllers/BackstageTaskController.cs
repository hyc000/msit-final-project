
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using prjCoreWebWantWant.Models;
using System.Runtime.ConstrainedExecution;
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

        #region TableEditable表

        //不分partialView的已上架
        public IActionResult TablesEditable()
        {
            var q = from t in _context.TaskLists
                    join tt in _context.TaskNameLists on t.TaskNameId equals tt.TaskNameId
                    where t.PublishOrNot == "立刻上架" && t.TaskNameId == tt.TaskNameId
                    select t;
            return View(q);
        }

        //選擇任務類別+已上架
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


        //已上架未上架的modal修改畫面
        
        public IActionResult Edit(int ? id)
        {
            if (id == null)
                return View("TablesEditable");

            NewIspanProjectContext _context = new NewIspanProjectContext();
            TaskList task = _context.TaskLists.FirstOrDefault(p => p.CaseId == id);
            if ( task==null)  
                return RedirectToAction("TablesEditable");

            return View(task);
                     
        }

        [HttpPost]
        public IActionResult Edit (TaskList tasknew)
        {
            NewIspanProjectContext _context = new NewIspanProjectContext();
            TaskList task = _context.TaskLists.FirstOrDefault(p => p.CaseId == tasknew.CaseId);

            if (task != null)
            {
                task.TaskTitle = tasknew.TaskTitle;
                task.TaskDetail=tasknew.TaskDetail;
                task.PublishOrNot = tasknew.PublishOrNot;          
            
                _context.SaveChanges();
            }
            return RedirectToAction("TablesEditable");
        }


        #endregion

        #region Approve表

        //app表的未處理tab，AllResume
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

        //app表未處理AllResume的錄取btn
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

        
        //approve表點選錄取btn後的已錄取tab
        public IActionResult ApproveAcceptPartialView(string category)
        {
            var query = (from app in _context.ApplicationLists
                         join task in _context.TaskLists on app.CaseId equals task.CaseId
                         join resume in _context.Resumes on app.ResumeId equals resume.ResumeId
                         join member in _context.MemberAccounts on resume.AccountId equals member.AccountId
                         join resumeskill in _context.ResumeSkills on resume.ResumeId equals resumeskill.ResumeId
                         join skill in _context.Skills on resumeskill.SkillId equals skill.SkillId
                         join resumecer in _context.ResumeCertificates on resume.ResumeId equals resumecer.ResumeId
                         join cer in _context.Certificates on resumecer.CertificateId equals cer.CertificateId

                         where app.CaseStatusId == 1

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
                         }).AsEnumerable()

                         .Where(app => app.TaskName == category )  // 此處是在客戶端進行過濾
                         .Distinct();


            //var viewModelList = query.ToList();

            return PartialView("ApproveAcceptPartialView", query.ToList());

            //        IQueryable<CApproveViewModel> query = _context.ApplicationLists
            //   .Join(
            //       _context.TaskLists,
            //       app => app.CaseId,
            //       task => task.CaseId,
            //       (app, task) => new { Application = app, Task = task }
            //   )
            //   .Join(
            //       _context.Resumes,
            //       combined => combined.Application.ResumeId,
            //       resume => resume.ResumeId,
            //       (combined, resume) => new { combined.Application, combined.Task, Resume = resume }
            //   )
            //   .Join(
            //       _context.MemberAccounts,
            //       combined => combined.Resume.AccountId,
            //       member => member.AccountId,
            //       (combined, member) => new { combined.Application, combined.Task, combined.Resume, Member = member }
            //   )
            //   .Join(
            //    _context.ResumeSkills,
            //    combined => combined.Resume.ResumeId,
            //    resumeskill => resumeskill.ResumeId,
            //    (combined, resumeskill) => new { combined.Application, combined.Task, combined.Resume, combined.Member, ResumeSkill = resumeskill }
            //)
            //.Join(
            //    _context.Skills,
            //    combined => combined.ResumeSkill.SkillId,
            //    skill => skill.SkillId,
            //    (combined, skill) => new { combined.Application, combined.Task, combined.Resume, combined.Member, combined.ResumeSkill, Skill = skill }
            //)
            //.Join(
            //    _context.ResumeCertificates,
            //    combined => combined.Resume.ResumeId,
            //    resumecer => resumecer.ResumeId,
            //    (combined, resumecer) => new { combined.Application, combined.Task, combined.Resume, combined.Member, combined.ResumeSkill, combined.Skill, ResumeCertificate = resumecer }
            //)
            //.Join(
            //    _context.Certificates,
            //    combined => combined.ResumeCertificate.CertificateId,
            //    cer => cer.CertificateId,
            //    (combined, cer) => new { combined.Application, combined.Task, combined.Resume, combined.Member, combined.ResumeSkill, combined.Skill, combined.ResumeCertificate, Certificate = cer }
            //)
            //.Join(
            //    _context.TaskNameLists,
            //    combined => combined.Task.TaskNameId,
            //        taskName => taskName.TaskNameId,
            //    (combined, taskName) => new { combined.Application, combined.Task, combined.Resume, combined.Member, combined.ResumeSkill, combined.Skill, combined.ResumeCertificate, combined.Certificate, TaskNameList=taskName}
            //   )

            //   .Where(combined => combined.Application.CaseStatusId == 1 && combined.TaskNameList.TaskName == category)
            //   .Select(combined => new CApproveViewModel
            //   {
            //       CaseId = combined.Task.CaseId,
            //       ResumeId = combined.Resume.ResumeId,
            //       TaskNameId = combined.Task.TaskNameId,
            //       CaseStatusId = combined.Application.CaseStatusId,
            //       Name = combined.Member.Name,
            //       SkillName = combined.Skill.SkillName, // Assuming you have the skill entity joined
            //       CertificateName = combined.Certificate.CertificateName, // Assuming you have the certificate entity joined
            //       Autobiography = combined.Resume.Autobiography,
            //       PublishStart = combined.Task.PublishStart,
            //       TaskStart = combined.Task.TaskStartDate,
            //       TaskTitle = combined.Task.TaskTitle,
            //       TaskDetail = combined.Task.TaskDetail
            //   });

            //        if (!string.IsNullOrEmpty(category))
            //        {
            //            query = query.Where(combined => combined.TaskName== category);
            //        }

            //        var viewModelList = query.ToList();

            //        return PartialView("ApproveAcceptPartialView", viewModelList);
        }


        //app表未處理AllResume的婉拒btn
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

        //approve表點選婉拒後的已婉拒tab
        public IActionResult ApproveRefusePartialView(string category)
        {
            var query = (from app in _context.ApplicationLists
                         join task in _context.TaskLists on app.CaseId equals task.CaseId
                         join resume in _context.Resumes on app.ResumeId equals resume.ResumeId
                         join member in _context.MemberAccounts on resume.AccountId equals member.AccountId
                         join resumeskill in _context.ResumeSkills on resume.ResumeId equals resumeskill.ResumeId
                         join skill in _context.Skills on resumeskill.SkillId equals skill.SkillId
                         join resumecer in _context.ResumeCertificates on resume.ResumeId equals resumecer.ResumeId
                         join cer in _context.Certificates on resumecer.CertificateId equals cer.CertificateId

                         where app.CaseStatusId == 2

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
                         }).AsEnumerable()

                         .Where(app => app.TaskName == category)  
                         .Distinct();

                return PartialView("ApproveAcceptPartialView", query.ToList());            
        }

        #endregion


        public IActionResult Publish(int? id)
        {
            TaskList task = _context.TaskLists.FirstOrDefault(p => p.CaseId == id);
            if (id != null)
            {
                task.PublishOrNot = "立刻上架";
                _context.SaveChanges();
            }
            return RedirectToAction("TablesEditable");
        }

        public IActionResult NoPublish(int? id)
        {
            TaskList task = _context.TaskLists.FirstOrDefault(p => p.CaseId == id);
            if (id != null)
            {
                task.PublishOrNot = "延後上架";
                _context.SaveChanges();
            }
            return RedirectToAction("TablesEditable");
        }

        //public IActionResult Form()
        //{
        //    return View("Form");
        //}

        //public IActionResult Form( CCreateTask createTask)
        //{  


        //    return View("Form");
        //}

        #region Form表 Create表
        public IActionResult Create()
        { 
            return View();      
        
        }
        [HttpPost]
        public IActionResult Create(TaskList tasklist, int selectedTaskNameId , int selectedTownId, int selectedPaymentId, int selectedPaymentDateId, int selectedSkillId, int selectedCerId)
        {

            tasklist.TaskNameId = selectedTaskNameId;
            tasklist.TownId = selectedTownId;
            tasklist.PaymentId = selectedPaymentId;
            tasklist.PaymentDateId = selectedPaymentDateId;
              
            _context.TaskLists.Add(tasklist);
            _context.SaveChanges();

                TaskSkill taskSkill = new TaskSkill()
                {
                    CaseId = tasklist.CaseId,   //CaseId是taskSkill的CaseId，後面是任務表的CaseId
                    SkillId = selectedSkillId
                };
                _context.Add(taskSkill);
                _context.SaveChanges();

                TaskCertificate taskCer = new TaskCertificate()
                {
                    CaseId = tasklist.CaseId,   //CaseId是taskCer的CaseId，後面是任務表的CaseId
                    CertficateId = selectedCerId
                };
                _context.Add(taskCer);
                _context.SaveChanges();


                 return RedirectToAction("Create");

        }

        //取得任務類型的selectIndex
        public IActionResult GetTaskNameId(string taskname)
        {
            var taskName = _context.TaskNameLists
                         .Where(a => a.TaskName == taskname)
                         .Select(c => c.TaskNameId);

            return Json(taskName);
        }
        //取得鄉鎮和城市的selectIndex
        public IActionResult GetTownId(string City, string District)
        {
            var townId = _context.Cities
                         .Where(a => a.City1 == City)
                         .SelectMany(c => c.Towns)
                         .Where(c => c.Town1 == District)
                         .Select(c => c.TownId);

            return Json(townId);
        }


        //取得支薪方式的selectIndex
        public IActionResult GetPaymentId(string payment)
        {
            var Payment = _context.Payments
                         .Where(a => a.Payment1 == payment)
                         .Select(c => c.PaymentId);

            return Json(Payment);
        }

        //取得支薪日的selectIndex
        public IActionResult GetPaymentDateId(string paymentDate)
        {
            var PaymentDate = _context.PaymentDates
                         .Where(a => a.PaymentDate1 == paymentDate)
                         .Select(c => c.PaymentDateId);

            return Json(PaymentDate);
        }

        //取得技能的selectIndex
        public IActionResult GetSkillId(string skillBig, string skillSmall)
        {
            var skillSmallId = _context.SkillTypes
                         .Where(a => a.SkillTypeName== skillBig)
                         .SelectMany(c => c.Skills)
                         .Where(c => c.SkillName == skillSmall)
                         .Select(c => c.SkillId);

            return Json(skillSmallId);
        }

        //取得證照的selectIndex
        public IActionResult GetCerId(string cerBig, string cerSmall)
        {
            var cerSmallId = _context.CertificateTypes
                         .Where(a => a.CertificateTypeName == cerBig)
                         .SelectMany(c => c.Certificates)
                         .Where(c => c.CertificateName == cerSmall)
                         .Select(c => c.CertificateId);

            return Json(cerSmallId);
        }


        public IActionResult Form(TaskList taskList)
        {          
            _context.TaskLists.Add(taskList);
            _context.SaveChanges();

            return View("Form");
         
        }

     
        #endregion

        //public IActionResult JobdetailBackstage()
        //{
        //    return View("JobdetailBackstage");
        //}





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



        //任務名稱
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


        //城市
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
