using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjCoreWantMember.ViewModels;
using prjCoreWebWantWant.Models;
using prjCoreWebWantWant.ViewModels;

namespace prjCoreWebWantWant.Controllers
{
    public class ExpertResumesController : Controller
    {
        private readonly NewIspanProjectContext _context;
        private int _memberID;
        private CExpertInfoViewModel _cexpertresume;
        private List <ExpertWork> _work;
        public ExpertResumesController(NewIspanProjectContext context)
        {
            _context = context;
            _cexpertresume = new CExpertInfoViewModel();
            _work= new List<ExpertWork>();

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
        public async Task<IActionResult> Resume(int? id)
        {
            CExperTaskFactory factory = new CExperTaskFactory(_context);
            CExpertShowResumesViewModel vm = new CExpertShowResumesViewModel();
            if (!id.HasValue || id.Value <= 0)
            {
                vm.專家姓名 = "無專家履歷";
              return View(vm);
            }
            //基本
            var qresume = _context.Resumes
                   .Where(a => a.ResumeId == id)
                   .FirstOrDefault();
            vm.專家ID = qresume.AccountId;
            vm.專家履歷ID = id.GetValueOrDefault();
            vm.專家姓名 = factory.MemberName(qresume.AccountId);
            vm.服務地區 = factory.TownID2Name(qresume.TownId);
            vm.履歷標題 = qresume.ResumeTitle;
            

            //履歷照片
            if (qresume.Photo != null)
            {
                vm.履歷照片 = qresume.Photo;
                
            }
          
            //專長
            var qskill = _context.ResumeSkills
                .Where(a => a.ResumeId == id)
                .Select(x => x.SkillId)
                 .ToList();

            if (qskill.Count > 0 && qskill[0] != null)
            {
                vm.專長1 =factory.SkillIDtoName(qskill[0]);
            }
            if (qskill.Count > 1 && qskill[1] != null)
            {
                vm.專長2 = factory.SkillIDtoName(qskill[1]); 
            }
            if (qskill.Count > 2 && qskill[2] != null)
            {
                vm.專長3 = factory.SkillIDtoName(qskill[2]);
            }
            //證照
            var qcertificates = _context.ResumeCertificates
            .Where(a => a.ResumeId == id)
            .Select(x => x.CertificateId)
             .ToList();

            if (qcertificates.Count > 0 && qcertificates[0] != null)
            {
                vm.證照1 = factory.CertificateDtoName(qcertificates[0]);
            }
            if (qcertificates.Count > 1 && qcertificates[1] != null)
            {
                vm.證照2 = factory.CertificateDtoName(qcertificates[1]);
            }
            if (qcertificates.Count > 2 && qcertificates[2] != null)
            {
                vm.證照3 = factory.CertificateDtoName(qcertificates[2]);
            }

            //詳細
            var qexresume = _context.ExpertResumes
                   .Where(a => a.ResumeId == id)
                   .FirstOrDefault();
            vm.專家介紹 = qexresume.Introduction;
            
            vm.聯絡方式 = qexresume.ContactMethod;

            
            if (qexresume.CommonPrice.HasValue)
            {
                vm.個人網站 = qexresume.WorksUrl;
            }
            else
            {
                vm.個人網站 = "沒有個人網站。";
            }
            if (qexresume.CommonPrice.HasValue)
            {
                vm.基本價格 = "$" + qexresume.CommonPrice.Value.ToString("N0");
            }
            else
            {
                vm.基本價格 = "$0"; // 或其他默認值
            }
            vm.收款方式 = qexresume.PaymentMethod;
            vm.提供服務 = qexresume.ServiceMethod;
            vm.常見問題 = qexresume.Problem;
            if (qexresume.HistoricalViews!=null)
            { vm.點閱次數 = qexresume.HistoricalViews.ToString(); }
            else
            {
                vm.點閱次數 = 0+"";
            }

            return View(vm);
        }




            // GET: ExpertResumes/Details/5
            public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //專家履歷
            var q1 = await _context.ExpertResumes
               .Include(e => e.Resume)
               .FirstOrDefaultAsync(m => m.ResumeId == id);

            //q1萬一NULL找不到的話
            _cexpertresume.expertResume = (q1 != null) ? q1 : _cexpertresume.expertResume;

            //專家作品集
            var qw = await _context.ExpertWorks
                 .Include(e => e.ExpertWorkLists.Where(x=>x.ResumeId == id))
                
                 .Select(x => x).ToListAsync();

            _work = (qw != null) ? qw : _work;

            //履歷
            var q2 = await _context.Resumes
             .Where(a => a.ResumeId == id)
             .FirstOrDefaultAsync();

            _cexpertresume.resume = (q2 != null) ? q2 : _cexpertresume.resume;




            //專家姓名
            var qa = await _context.MemberAccounts
                .Where(x => x.AccountId == _cexpertresume.resume.AccountId)
             .FirstOrDefaultAsync();

            _cexpertresume.memberAccount = (qa != null) ? qa : _cexpertresume.memberAccount;

            //專家證照

            var qc = await _context.Certificates
                .Include(x => x.ResumeCertificates.Where(r => r.ResumeId == id))
            .FirstOrDefaultAsync();

            _cexpertresume.certificate = (qc != null) ? qc : _cexpertresume.certificate;

            var qct = await _context.CertificateTypes
              .Include(x => x.Certificates)
              .ThenInclude(x => x.ResumeCertificates.Where(r => r.ResumeId == id))
          .FirstOrDefaultAsync();

            _cexpertresume.certificatetype = (qct != null) ? qct : _cexpertresume.certificatetype;

            //專家專長
            //TODO
            var qs = await _context.Skills
                .Include(x => x.ResumeSkills.Where(r => r.ResumeId == id))
            .FirstOrDefaultAsync();

         


            _cexpertresume.skill = (qs != null) ? qs : _cexpertresume.skill;

            var qst = await _context.SkillTypes
              .Include(x => x.Skills)
              .ThenInclude(x => x.ResumeSkills.Where(r => r.ResumeId == id))
          .FirstOrDefaultAsync();

            _cexpertresume.skillType = (qst != null) ? qst : _cexpertresume.skillType;

            return View(_cexpertresume);
        }
        public IActionResult GetImage()
        {

            //履歷照片
            if (_cexpertresume.resume != null && _cexpertresume.resume.Photo != null)

            {
                byte[]? img = _cexpertresume.resume.Photo;
                return File(img, "image/jpeg");
            }
            return File("~/Expert/images/3.jpg", "image/jpeg");
        }
       
        //public IActionResult WorksImage()
        //{
        //    List<FileResult> files = new List<FileResult>();
        //    if (_work != null)
        //    {
        //        foreach (var item in _work)
        //        {
        //            if (item.WorksPhoto != null)
        //            {
        //                files.Add(File(item.WorksPhoto, "image/jpeg"));
        //            }

        //        }

        //    }
        //    else
        //    {
        //        return File("~/Expert/images/2.webp", "image/jpeg");
        //    }

        //    return Json(files);


        //}

        public async Task<IActionResult> Insert() {
           
            return View();
            
        }
        [HttpPost]
        public async Task<IActionResult> Insert(CExpertResumesViewModel vm)
        {
            _memberID= GetMemberIDFromSession();
            if (_memberID == 0)
            {
                TempData["message"] = "請先登入";
                return RedirectToAction("ExpertMemberPage", "Expert");
            }
            
            try
            {
                int num = _context.Resumes
                    .Count(x => x.AccountId == _memberID && x.IsExpertOrNot == true);

                //if (num >= 3)//履歷數量<3
                //{
                //    TempData["message"] = "履歷份數已達上限，請刪除其他履歷才能新增";
                //    return RedirectToAction("ExpertMemberPage", "Expert");
                //}
                //else
                //{
                    if (vm != null)
                    {

                    //基本履歷&找流水號resumeID
                    int newResumeId = await AddResume(vm);

                        //專家履歷
                        AddExpertResume(vm, newResumeId);
                        //SKILL
                        AddSkills(vm, newResumeId);
                        //證照
                        AddCertificates(vm, newResumeId);

                     

                        //新增作品
                        DateTime dataCreateDate = DateTime.Now;//現在時間
                       
                            List<int> newExpertWorkId = await AddExpertWork(vm, dataCreateDate);

                        // 作品表單

                            await AddExpertWorkList(newExpertWorkId, newResumeId);

                        TempData["message"] = "新增成功";
                        //成功回去
                      //  return RedirectToAction("ExpertMemberPage", "Expert");

                    }

                    //失敗回去
                    TempData["message"] = "新增失敗";
                    return RedirectToAction("ExpertMemberPage", "Expert");
                //}
            }

            catch (Exception ex)
            {
                // 处理异常，例如记录日志等
                TempData["message"] = "發生錯誤：" + ex.Message;
                return RedirectToAction("ExpertMemberPage", "Expert");
            }
        
        }
        private async Task<byte[]> ConvertFileToBytes(IFormFile file)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
        private async Task<int> AddResume(CExpertResumesViewModel vm)
        {
            DateTime date = DateTime.Now;
            CExperTaskFactory factory = new CExperTaskFactory(_context);

            int? townid = factory.TownName(vm.服務地區);
            
            Resume resume = new Resume
            {
                AccountId = _memberID,
                TownId = townid,
                Photo = await ConvertFileToBytes(vm.履歷照片),
                IsExpertOrNot = true,//TRUE
                CaseStatusId = 23,//23顯示履歷
                ResumeTitle = vm.履歷標題,
                DataModifyDate= date.ToString()
            };

            _context.Resumes.Add(resume);
          await _context.SaveChangesAsync();
            return resume.ResumeId;
        }

        private async Task AddSkills(CExpertResumesViewModel vm, int newResumeId)
        {
            CExperTaskFactory factory = new CExperTaskFactory(_context);

            async Task AddSkillIfNotNull(int? skillId)
            {
                if (skillId != null)
                {
                    ResumeSkill resumeskill = new ResumeSkill
                    {
                        SkillId = skillId.Value,
                        ResumeId = newResumeId
                    };
                    _context.ResumeSkills.Add(resumeskill);
                   await _context.SaveChangesAsync();
                }
            }

            int? SkillID1 = factory.SkillName(vm.專長細項1);
            await AddSkillIfNotNull(SkillID1);
            int? SkillID2 = factory.SkillName(vm.專長細項2);
            await AddSkillIfNotNull(SkillID2);
            int? SkillID3 = factory.SkillName(vm.專長細項3);
            await AddSkillIfNotNull(SkillID3);

        }


        private async Task AddCertificates(CExpertResumesViewModel vm, int newResumeId)
        {
            CExperTaskFactory factory = new CExperTaskFactory(_context);
            async Task AddCertificatesNotNull(int? certificateId)
            {
                if (certificateId != null)
                {
                    ResumeCertificate resumecertificate = new ResumeCertificate
                    {
                        CertificateId = certificateId.Value,
                        ResumeId = newResumeId
                    };
                    _context.ResumeCertificates.Add(resumecertificate);
                   await _context.SaveChangesAsync();
                }
            }
            int? CertificateID1 = factory.CertificateName(vm.證照細項1);
            await AddCertificatesNotNull(CertificateID1);
            int? CertificateID2 = factory.CertificateName(vm.證照細項2);
            await AddCertificatesNotNull(CertificateID2);
            int? CertificateID3 = factory.CertificateName(vm.證照細項3);
            await AddCertificatesNotNull(CertificateID3);
          
        }

        private async Task AddExpertResume(CExpertResumesViewModel vm, int newResumeId)
        {
           
            ExpertResume expertresume = new ExpertResume
            {
                ResumeId = newResumeId,
                Introduction = vm.專家介紹,
                WorksUrl = vm.個人網站,
                ContactMethod = vm.聯絡方式,
                ServiceMethod = vm.提供服務,
                PaymentMethod = vm.收款方式,
                Problem = vm.常見問題,
                CommonPrice = vm.基本價格
               
            };
            _context.ExpertResumes.Add(expertresume);
            await _context.SaveChangesAsync();
        
        }

        private async Task<List<int>> AddExpertWork(CExpertResumesViewModel vm, DateTime dataCreateDate)
        {
            List<int> WorksIds = new List<int> { };
            if (vm.作品集.Count>0)
            {
                
                for(int i = 0; i < vm.作品集.Count; i++)
                {
                    ExpertWork expertwork = new ExpertWork
                    {
                        WorksPhoto = await ConvertFileToBytes(vm.作品圖片[i]),
                        Workname = vm.作品集[i] + dataCreateDate.ToString("yyyyMMddHHmmssff"),
                        DataCreateDate = dataCreateDate,
                        WorkTitle = vm.作品集[i]
                    };
                    _context.ExpertWorks.Add(expertwork);
                    await _context.SaveChangesAsync();
                    int worlid = expertwork.WorksId;
                    WorksIds.Add(worlid);
                }
            }
         

            return WorksIds;
        }

        private async Task AddExpertWorkList(List<int> newexpertworkId, int newResumeId)
        {
            foreach(int i in newexpertworkId)
            {
                ExpertWorkList expertworklist = new ExpertWorkList
                {
                    WorksId = i,
                    ResumeId = newResumeId
                };
                _context.ExpertWorkLists.Add(expertworklist);
            }
            
           await _context.SaveChangesAsync();
        }

    }
}
