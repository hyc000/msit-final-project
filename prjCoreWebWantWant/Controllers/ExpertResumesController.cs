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
        //public  IActionResult GetImage()
        //{

        //    //履歷照片
        //    if (_cexpertresume.resume != null&&_cexpertresume.resume.Photo != null)

        //    {
        //        byte[]? img = _cexpertresume.resume.Photo;
        //        return File(img, "image/jpeg");
        //    }
        //    return File("~/Expert/images/3.jpg", "image/jpeg");
        //}
        //public  IActionResult WorksImage()
        //{
        //    List<FileResult> files = new List<FileResult>();
        //    if (_work != null)
        //    {
        //        foreach(var item in _work)
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

                if (num >= 3)//履歷數量<3
                {
                    TempData["message"] = "履歷份數已達上限，請刪除其他履歷才能新增";
                    return RedirectToAction("ExpertMemberPage", "Expert");
                }
                else
                {
                    if (vm != null)
                    {
                        
                        //找流水號resumeID
                        int newResumeId = await AddResume(vm);

                        //專家履歷
                        AddExpertResume(vm, newResumeId);
                        //SKILL
                        AddSkills(vm, newResumeId);
                        //證照
                        AddCertificates(vm, newResumeId);

                     

                        //新增作品
                        DateTime dataCreateDate = DateTime.Now;//現在時間
                        List<int> newExpertWorkIds = new List<int>();

                        
                        for (int i = 0; i < newExpertWorkIds.Count(); i++) 
                        {
                            int newExpertWorkId = await AddExpertWork(vm, dataCreateDate);
                            if (newExpertWorkId != -1)
                            {
                                newExpertWorkIds.Add(newExpertWorkId);
                            }
                        }

                        // 作品表單
                        foreach (int expertWorkId in newExpertWorkIds)
                        {
                            await AddExpertWorkList(expertWorkId, newResumeId);
                        }



                        TempData["message"] = "新增成功";
                        //成功回去
                      //  return RedirectToAction("ExpertMemberPage", "Expert");



                    }

                    //失敗回去
                    TempData["message"] = "新增失敗";
                    return RedirectToAction("ExpertMemberPage", "Expert");
                }
            }

            catch (Exception ex)
            {
                // 处理异常，例如记录日志等
                TempData["message"] = "發生錯誤：" + ex.Message;
                return RedirectToAction("ExpertMemberPage", "Expert");
            }
        
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
                Photo = vm.履歷照片,
                IsExpertOrNot = true,//TRUE
                CaseStatusId = 23,//23顯示履歷
                ResumeTitle = vm.履歷標題,
                DataModifyDate= date.ToString()
            };

            _context.Resumes.Add(resume);
          //  await _context.SaveChangesAsync();--080--
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
                   // await _context.SaveChangesAsync();--080--
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
                  //  await _context.SaveChangesAsync();--080--
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
            //await _context.SaveChangesAsync();--080--
        
        }

        private async Task<int> AddExpertWork(CExpertResumesViewModel vm, DateTime dataCreateDate)
        {
            
            if (vm.作品圖片 != null)
            {
                ExpertWork expertwork = new ExpertWork
                {
                    WorksPhoto = vm.作品圖片,
                    Workname = vm.作品名 + dataCreateDate.ToString("yyyyMMddHHmmssff"),
                    DataCreateDate = dataCreateDate,
                    // WorkTitle =vm.作品名
                };
                _context.ExpertWorks.Add(expertwork);
              //  await _context.SaveChangesAsync();--080--

                return expertwork.WorksId;
                
            }
            return -1;
        }

        private async Task AddExpertWorkList(int newexpertworkId, int newResumeId)
        {
            ExpertWorkList expertworklist = new ExpertWorkList
            {
                WorksId = newexpertworkId,
                ResumeId = newResumeId
            };
            _context.ExpertWorkLists.Add(expertworklist);
           // await _context.SaveChangesAsync();--080--
        }

    }
}
