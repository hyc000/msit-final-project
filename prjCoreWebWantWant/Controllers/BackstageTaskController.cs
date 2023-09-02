using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using prjCoreWantMember.ViewModels;
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
        private CTaskDetailFrontandBackstage _CTaskFrontAndBack;
        public BackstageTaskController(NewIspanProjectContext context, IWebHostEnvironment host)
        {
            _context = context;
            _host = host;
        }

        #region TableEditable表

        //把tableeditable叫出來
        public IActionResult TablesEditable()
        {
            //var q = from t in _context.TaskLists
            //        join tt in _context.TaskNameLists on t.TaskNameId equals tt.TaskNameId
            //        where t.PublishOrNot == "立刻上架" && t.TaskNameId == tt.TaskNameId
            //        select t;
            return View();

            //IEnumerable<TaskList> datas = null;
            //if (string.IsNullOrEmpty(vm.txtKeyword))
            //{
            //    datas = _context.TaskLists
            //            .Where(t => t.TaskName.TaskName == category && t.PublishOrNot == "立刻上架");
            //}
            //else
            //{
            //    datas = _context.TaskLists.Where(t => t.TaskTitle.ToUpper().Contains(vm.txtKeyword.ToUpper()) && t.PublishOrNot == "立刻上架" ||
            //    t.TaskDetail.ToUpper().Contains(vm.txtKeyword.ToUpper()) && t.PublishOrNot == "立刻上架" ||
            //    t.Address.ToUpper().Contains(vm.txtKeyword.ToUpper()) && t.PublishOrNot == "立刻上架"
            //   );
            //}
            //return PartialView( "PartialPublish",datas);
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




        //選擇任務類別+keyword+已上架
        public IActionResult PartialPublish(string category, CKeywordViewModel vm)
        {
            if (vm.txtKeyword == null)
            {
                vm.txtKeyword = "";
            }

            if (category == null)
            {
                var all = _context.TaskLists.
                Where(t => t.PublishOrNot == "立刻上架"
                && (t.TaskTitle.ToUpper().Contains(vm.txtKeyword.ToUpper())
                  || t.TaskDetail.ToUpper().Contains(vm.txtKeyword.ToUpper()))
                );
                return PartialView(all);  //換頁數? .Take()
            }

            var taskName = _context.TaskLists.
                Where(t => t.TaskName.TaskName == category
                && t.PublishOrNot == "立刻上架"
                && (t.TaskTitle.ToUpper().Contains(vm.txtKeyword.ToUpper())
                || t.TaskDetail.ToUpper().Contains(vm.txtKeyword.ToUpper()))
                );
            return PartialView(taskName);
        }

        //選擇任務類別+keyword+未上架
        public IActionResult PartialNoPublish(string category, CKeywordViewModel vm)
        {
            if (vm.txtKeyword == null)
            {
                vm.txtKeyword = "";
            }

            if (category == null)
            {
                var all = _context.TaskLists.
                Where(t => t.PublishOrNot == "延後上架"
                && (t.TaskTitle.ToUpper().Contains(vm.txtKeyword.ToUpper())
                  || t.TaskDetail.ToUpper().Contains(vm.txtKeyword.ToUpper()))
                );
                return PartialView(all);  //換頁數? .Take()
            }

            var taskName = _context.TaskLists.
                Where(t => t.TaskName.TaskName == category
                && t.PublishOrNot == "延後上架"
                && (t.TaskTitle.ToUpper().Contains(vm.txtKeyword.ToUpper())
                || t.TaskDetail.ToUpper().Contains(vm.txtKeyword.ToUpper()))
                );
            return PartialView(taskName);
        }

        //立刻上架的keyword
        public IActionResult publishKeyword(CKeywordViewModel vm, string category)
        {
            IEnumerable<TaskList> datas = null;
            if (string.IsNullOrEmpty(vm.txtKeyword))
            {
                datas = _context.TaskLists
                        .Where(t => t.TaskName.TaskName == category && t.PublishOrNot == "立刻上架");
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

        //改狀態為立刻上架
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
        //改狀態為延後上架
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

        //改狀態為空值=UI的刪除
        public IActionResult PubNull(int? id)
        {
            TaskList task = _context.TaskLists.FirstOrDefault(p => p.CaseId == id);
            if (id != null)
            {
                task.PublishOrNot = "";
                _context.SaveChanges();
            }
            return RedirectToAction("TablesEditable");
        }


        public IActionResult Edit(int? id)
        {
            if (id == null)
                return View("TablesEditable");

            NewIspanProjectContext _context = new NewIspanProjectContext();

            TaskList task = _context.TaskLists           
             .Include(c => c.Town.City).Where(t => t.CaseId == id).FirstOrDefault();            
            
            //_context.TaskLists.FirstOrDefault(p => p.CaseId == id);

            if (task == null)
                return RedirectToAction("TablesEditable");

            return View(task);
        }

        [HttpPost]
        public IActionResult Edit(TaskList tasknew , int  selectedTownID, IFormFile imageFile)
        {
            NewIspanProjectContext _context = new NewIspanProjectContext();
            TaskList task = _context.TaskLists.FirstOrDefault(p => p.CaseId == tasknew.CaseId);
            if (task != null)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                string filePath = Path.Combine(_host.WebRootPath, "backstage1", "TaskPhoto", imageFile.FileName);
                //var imagePath = "~/Backstage/img/" + Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                string imagePath = "/backstage1/TaskPhoto/" + imageFile.FileName;

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                    imageFile.CopyTo(fileStream);
                    }
                task.PhotoPath = imagePath;
                }
                task.TaskTitle = tasknew.TaskTitle;
                task.TaskDetail = tasknew.TaskDetail;
                task.PublishOrNot = tasknew.PublishOrNot;
                task.PayFrom=tasknew.PayFrom;
                task.TaskStartDate = tasknew.TaskStartDate;
                task.TownId=selectedTownID;

                _context.SaveChanges();
            }
            return RedirectToAction("TablesEditable");
        }

        //點選已上架未上架的詳細任務畫面
        public IActionResult JobDetail(int? id)
        {
            var q = _context.TaskLists
                //.Include(s => s.TaskSkills)
                //.ThenInclude(s=>s.Skill)
                //.Include(c=>c.TaskCertificates)
                //.ThenInclude(c=>c.Certficate)
                //.Include(p=>p.TaskPhotos)
                //.ThenInclude(p=>p.Photo)
                //.Where(ss => ss.CaseId == id)


                //.Include(t=>t.TaskCertificates).Where(tt=>tt.CaseId==id)
                //.Include(p=>p.TaskPhotos).Where(pp=>pp.CaseId==id)沒用的三行
                .Include(c => c.Town.City).Where(t => t.CaseId == id).FirstOrDefault();

            //var q = from p in _context.TaskLists

            //        where p.CaseId == id

            return View(q);
        }

        //public IActionResult yes()
        //{
        //    var q = from app in _context.ApplicationLists.AsEnumerable()

        //            where app.TaskList.TaskNameID == (int)this.Tag && app.CaseStatusID == 21

        //            select app;
        //}

        //jobDetail
        //public async Task<IActionResult>JobDetail(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    //任務地點
        //    var q = await _context.TaskLists
        //     .Include(c => c.Town.City).FirstOrDefaultAsync(t => t.CaseId == id);

        //    ////履歷
        //    //var q2 = await _context.Resumes
        //    // .Where(a => a.ResumeId == id)
        //    // .FirstOrDefaultAsync();

        //    //_cexpertresume.resume = (q2 != null) ? q2 : _cexpertresume.resume;

        //    //刊登者姓名
        //    var qa = await _context.MemberAccounts
        //        .Where(x => x.AccountId == _CTaskFrontAndBack.AccountId)
        //     .FirstOrDefaultAsync();

        //    _CTaskFrontAndBack.memberAccount = (qa != null) ? qa : _CTaskFrontAndBack.memberAccount;

        //    //證照
        //    var qc = await _context.Certificates
        //        .Include(x => x.TaskCertificates.Where(r => r.CaseId == id))
        //    .FirstOrDefaultAsync();

        //    _CTaskFrontAndBack.certificate = (qc != null) ? qc : _CTaskFrontAndBack.certificate;

        //    var qct = await _context.CertificateTypes
        //      .Include(x => x.Certificates)
        //      .ThenInclude(x => x.TaskCertificates.Where(r => r.CaseId == id))
        //  .FirstOrDefaultAsync();

        //    _CTaskFrontAndBack.certificateType = (qct != null) ? qct : _CTaskFrontAndBack.certificateType;

        //    //專長
        //    var qs = await _context.Skills
        //        .Include(x => x.TaskSkills.Where(r => r.CaseId == id))
        //    .FirstOrDefaultAsync();

        //    _CTaskFrontAndBack.skill = (qs != null) ? qs : _CTaskFrontAndBack.skill;

        //    var qst = await _context.SkillTypes
        //      .Include(x => x.Skills)
        //      .ThenInclude(x => x.TaskSkills.Where(r => r.CaseId == id))
        //  .FirstOrDefaultAsync();

        //    _CTaskFrontAndBack.skillType = (qst != null) ? qst : _CTaskFrontAndBack.skillType;

        //    return View(_CTaskFrontAndBack);
        //}

        //用viewmodel的方法，想叫出多技能多專長但是大失敗
        //public IActionResult JobDetail(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    //任務地點
        //    var q = _context.TaskLists
        //     .Include(c => c.Town.City)
        //     .FirstOrDefault(t => t.CaseId == id);

        //    ////履歷
        //    //var q2 = await _context.Resumes
        //    // .Where(a => a.ResumeId == id)
        //    // .FirstOrDefaultAsync();

        //    //_cexpertresume.resume = (q2 != null) ? q2 : _cexpertresume.resume;

        //    //刊登者姓名
        //    //var qa =  _context.MemberAccounts
        //    //    .Where(x => x.AccountId == _CTaskFrontAndBack.AccountId)
        //    // .FirstOrDefault();

        //    //_CTaskFrontAndBack.memberAccount = (qa != null) ? qa : _CTaskFrontAndBack.memberAccount;

        //    //證照
        //    var qc = _context.Certificates
        //        .Include(x => x.TaskCertificates.Where(r => r.CaseId == id))
        //    .FirstOrDefault();

        //    //_CTaskFrontAndBack.cer = (qc != null) ? qc : _CTaskFrontAndBack.cer;

        //    var qct = _context.CertificateTypes
        //      .Include(x => x.Certificates)
        //      .ThenInclude(x => x.TaskCertificates.Where(r => r.CaseId == id))
        //  .FirstOrDefault();

        //   // _CTaskFrontAndBack.certificateType = (qct != null) ? qct : _CTaskFrontAndBack.certificateType;

        //    //專長
        //    var qs = _context.Skills
        //        .Include(x => x.TaskSkills.Where(r => r.CaseId == id))
        //    .FirstOrDefault();

        //    //_CTaskFrontAndBack.skill = (qs != null) ? qs : _CTaskFrontAndBack.skill;

        //    var qst =  _context.SkillTypes
        //      .Include(x => x.Skills)
        //      .ThenInclude(x => x.TaskSkills.Where(r => r.CaseId == id))
        //  .FirstOrDefault();

        //  //  _CTaskFrontAndBack.skillType = (qst != null) ? qst : _CTaskFrontAndBack.skillType;

        //    return View(_CTaskFrontAndBack);
        //}

        #endregion

        #region Approve表

        //把Approve表叫出來
        public IActionResult Approve()
        {
            return View();
        }

        //app表的未處理tab AllResume+選擇任務類型+keyword
        public IActionResult ApproveAllResumePartialView(string category, CKeywordViewModel vm)
        {
            //if (vm.txtKeyword == null)
            //{
            //    vm.txtKeyword = "";
            //}

            //if (category == null)
            //{
            //    var all = (from app in _context.ApplicationLists
            //               join task in _context.TaskLists on app.CaseId equals task.CaseId
            //               join resume in _context.Resumes on app.ResumeId equals resume.ResumeId
            //               join member in _context.MemberAccounts on resume.AccountId equals member.AccountId
            //               join resumeskill in _context.ResumeSkills on resume.ResumeId equals resumeskill.ResumeId
            //               join skill in _context.Skills on resumeskill.SkillId equals skill.SkillId
            //               join resumecer in _context.ResumeCertificates on resume.ResumeId equals resumecer.ResumeId
            //               join cer in _context.Certificates on resumecer.CertificateId equals cer.CertificateId
            //               //join taskname in _context.TaskNameLists on task.TaskNameId equals taskname.TaskNameId

            //                 where app.CaseStatusId == 21 /* && task.AccountId == 66*/
            //                                                                     //林禮書的案件應該有3筆 但是加上條件後顯示0筆
            //               select new CApproveViewModel
            //               {
            //                   CaseId = task.CaseId,
            //                   ResumeId = resume.ResumeId,
            //                   TaskNameId = task.TaskNameId,
            //                   CaseStatusId = app.CaseStatusId,
            //                   Name = member.Name,
            //                   SkillName = skill.SkillName,
            //                   CertificateName = cer.CertificateName,
            //                   Autobiography = resume.Autobiography,
            //                   PublishStart = task.PublishStart,
            //                   TaskStart = task.TaskStartDate,
            //                   TaskTitle = task.TaskTitle,
            //                   TaskDetail = task.TaskDetail,
            //                   //TaskName = taskname.TaskName


            //               }).AsEnumerable()

            //    .Where(app => app.TaskTitle.ToUpper().Contains(vm.txtKeyword.ToUpper())
            //               || app.TaskDetail.ToUpper().Contains(vm.txtKeyword.ToUpper()))

            //    .Distinct();

            //    return PartialView(all);  //換頁數? .Take()

            //}

            //var query = (from app in _context.ApplicationLists
            //             join task in _context.TaskLists on app.CaseId equals task.CaseId
            //             join resume in _context.Resumes on app.ResumeId equals resume.ResumeId
            //             join member in _context.MemberAccounts on resume.AccountId equals member.AccountId
            //             join resumeskill in _context.ResumeSkills on resume.ResumeId equals resumeskill.ResumeId
            //             join skill in _context.Skills on resumeskill.SkillId equals skill.SkillId
            //             join resumecer in _context.ResumeCertificates on resume.ResumeId equals resumecer.ResumeId
            //             join cer in _context.Certificates on resumecer.CertificateId equals cer.CertificateId
            //             join taskname in _context.TaskNameLists on task.TaskNameId equals taskname.TaskNameId

            //             where app.CaseStatusId == 21

            //             select new CApproveViewModel
            //             {
            //                 CaseId = task.CaseId,
            //                 ResumeId = resume.ResumeId,
            //                 TaskNameId = task.TaskNameId,
            //                 CaseStatusId = app.CaseStatusId,
            //                 Name = member.Name,
            //                 SkillName = skill.SkillName,
            //                 CertificateName = cer.CertificateName,
            //                 Autobiography = resume.Autobiography,
            //                 PublishStart = task.PublishStart,
            //                 TaskStart = task.TaskStartDate,
            //                 TaskTitle = task.TaskTitle,
            //                 TaskDetail = task.TaskDetail,
            //                 TaskName = taskname.TaskName
            //             }).AsEnumerable()

            //             .Where(a => a.TaskName == category
            //                  && (a.TaskTitle.ToUpper().Contains(vm.txtKeyword.ToUpper())
            //                        || a.TaskDetail.ToUpper().Contains(vm.txtKeyword.ToUpper())))
            //             .GroupBy(app => app.ResumeId)
            //             .Select(g => g.First());

            //return PartialView(query);


            if (vm.txtKeyword == null)
            {
                vm.txtKeyword = "";
            }

            //聚合技能和證照
            var resumesWithSkillsAndCerts =
                from resume in _context.Resumes
                join resumeskill in _context.ResumeSkills on resume.ResumeId equals resumeskill.ResumeId
                join skill in _context.Skills on resumeskill.SkillId equals skill.SkillId
                join resumecer in _context.ResumeCertificates on resume.ResumeId equals resumecer.ResumeId
                join cer in _context.Certificates on resumecer.CertificateId equals cer.CertificateId
                group new { skill.SkillName, cer.CertificateName } by resume.ResumeId into grouped
                select new
                {
                    ResumeId = grouped.Key,
                    Skills = string.Join(", ", grouped.Select(x => x.SkillName).Distinct()),
                    Certificates = string.Join(", ", grouped.Select(x => x.CertificateName).Distinct())
                };


            var query =
                from app in _context.ApplicationLists
                join task in _context.TaskLists on app.CaseId equals task.CaseId
                join resume in _context.Resumes on app.ResumeId equals resume.ResumeId
                join member in _context.MemberAccounts on resume.AccountId equals member.AccountId
                join resumeSkillsCerts in resumesWithSkillsAndCerts on resume.ResumeId equals resumeSkillsCerts.ResumeId
                join taskname in _context.TaskNameLists on task.TaskNameId equals taskname.TaskNameId
                where app.CaseStatusId == 21
                select new CApproveViewModel
                {
                    CaseId = task.CaseId,
                    ResumeId = resume.ResumeId,
                    TaskNameId = task.TaskNameId,
                    CaseStatusId = app.CaseStatusId,
                    Name = member.Name,
                    SkillName = resumeSkillsCerts.Skills,
                    CertificateName = resumeSkillsCerts.Certificates,
                    Autobiography = resume.Autobiography,
                    PublishStart = task.PublishStart,
                    TaskStart = task.TaskStartDate,
                    TaskTitle = task.TaskTitle,
                    TaskDetail = task.TaskDetail,
                    TaskName = taskname.TaskName
                };

            // Filter by category and keyword
            if (category != null)
            {
                query = query.Where(a => a.TaskName == category
                            && (a.TaskTitle.ToUpper().Contains(vm.txtKeyword.ToUpper())
                                || a.TaskDetail.ToUpper().Contains(vm.txtKeyword.ToUpper())))
                             .GroupBy(app => app.ResumeId)
                             .Select(g => g.First());
            }
            else
            {
                query = query.Where(app => app.TaskTitle.ToUpper().Contains(vm.txtKeyword.ToUpper())
                                    || app.TaskDetail.ToUpper().Contains(vm.txtKeyword.ToUpper()))
                             .Distinct();
            }

            return PartialView(query);

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
                         join taskname in _context.TaskNameLists on task.TaskNameId equals taskname.TaskNameId

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
                             TaskDetail = task.TaskDetail,
                             TaskName = taskname.TaskName
                         }).AsEnumerable()

                         .Where(app => app.TaskName == category)  // 此處是在客戶端進行過濾
                         .Distinct();

            return PartialView("ApproveAcceptPartialView", query.ToList());

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
                         join resumeskill in _context.ResumeSkills on resume.ResumeId equals resumeskill.ResumeId //groupby
                         join skill in _context.Skills on resumeskill.SkillId equals skill.SkillId
                         join resumecer in _context.ResumeCertificates on resume.ResumeId equals resumecer.ResumeId //groupby
                         join cer in _context.Certificates on resumecer.CertificateId equals cer.CertificateId
                         join taskname in _context.TaskNameLists on task.TaskNameId equals taskname.TaskNameId

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
                             TaskDetail = task.TaskDetail,
                             TaskName = taskname.TaskName
                         }).AsEnumerable()

                         .Where(app => app.TaskName == category)
                          .GroupBy(app => app.ResumeId)
                           .Select(g => g.First());

            return PartialView("ApproveAcceptPartialView", query.ToList());
        }

        #endregion

        #region Form表 Create表
        public IActionResult Create()
        {
            return View();

        }
        [HttpPost]
        public IActionResult Create(TaskList tasklist, int selectedTaskNameId, int selectedTownId, int selectedPaymentId, int selectedPaymentDateId, int selectedSkillId, int selectedCerId, /*byte selectedPhoto,*/ string publishornot, IFormFile imageFile)
        {
            tasklist.TaskNameId = selectedTaskNameId;
            tasklist.TownId = selectedTownId;
            tasklist.PaymentId = selectedPaymentId;
            tasklist.PaymentDateId = selectedPaymentDateId;
            tasklist.PublishOrNot = publishornot;
            tasklist.DataModifyDate = DateTime.Now.Date.ToString("yyyy-MM-dd");
            if (imageFile != null && imageFile.Length > 0)
            {
                string filePath = Path.Combine(_host.WebRootPath, "backstage1", "TaskPhoto", imageFile.FileName);
                //var imagePath = "~/Backstage/img/" + Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                string imagePath = "/backstage1/TaskPhoto/" + imageFile.FileName;

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.CopyTo(fileStream);
                }
                tasklist.PhotoPath = imagePath;
            }

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

        //取照片
        public IActionResult GetImage(int? CaseID)
        {
            ////用find方法，不要用where.firstordefault，find會直接找pk
            //TaskPhoto? taskPhoto = _context.TaskPhotos.Find(CaseID);
            //byte[]? img = taskPhoto.Photo;
            //return File(img, "image/jpeg");  //file裡面的參數也有別的可選ex.text

            if (CaseID == null)
            {
                return NotFound(); // 或者其他適當的處理方式
            }

            TaskPhoto taskPhoto = _context.TaskPhotos.Find(CaseID);
            if (taskPhoto == null || taskPhoto.Photo == null)
            {
                return NotFound(); // 或者其他適當的處理方式
            }

            byte[] img = taskPhoto.Photo;
            return File(img, "image/jpeg");
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
            _context.TaskLists.Add(taskList);
            _context.SaveChanges();

            return View("Form");

        }

        #endregion

        #region 一堆select

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
                         .Where(a => a.SkillTypeName == skillBig)
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

        //任務名稱
        public IActionResult TaskName()
        {
            var taskName = _context.TaskNameLists.Select(c => c.TaskName).Distinct();

            return Json(taskName);
        }

        //支薪方式
        public IActionResult Payment()
        {
            var payment = _context.Payments.Select(c => c.Payment1).Distinct();

            return Json(payment);
        }

        //支薪日
        public IActionResult PaymentDate()
        {
            var paymentDate = _context.PaymentDates.Select(c => c.PaymentDate1).Distinct();

            return Json(paymentDate);
        }

        //城市
        public IActionResult Cities()
        {
            var cities = _context.Cities.Select(c => c.City1).Distinct();

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

        #endregion



    }
}
