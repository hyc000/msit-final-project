using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjCoreWantMember.ViewModels;
using prjCoreWebWantWant.Models;

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

            //Session取得
            _memberID = 5; //假資料要刪喔

            // int memberID ;           

            //等會員ID可以傳遞再打開，sessionKey要跟SET的時候依樣
            //if (HttpContext.Session.Keys.Contains(sessionKey))
            //{
            //    memberID = HttpContext.Session.GetString(sessionKey);

            //}
            //   _memberID = memberID;
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
        public  IActionResult GetImage()
        {
            
            //履歷照片
            if (_cexpertresume.resume != null&&_cexpertresume.resume.Photo != null)
           
            {
                byte[]? img = _cexpertresume.resume.Photo;
                return File(img, "image/jpeg");
            }
            return File("~/Expert/images/3.jpg", "image/jpeg");
        }
        public  IActionResult WorksImage()
        {
            List<FileResult> files = new List<FileResult>();
            if (_work != null)
            {
                foreach(var item in _work)
                {
                    if (item.WorksPhoto != null)
                    {
                        files.Add(File(item.WorksPhoto, "image/jpeg"));
                    }
                   
                }

            }
            else
            {
                return File("~/Expert/images/2.webp", "image/jpeg");
            }

            return Json(files);

          
        }


        public IActionResult StarRating(int id)
        {
            int accountid = _context.Resumes
                .Where(x => x.ResumeId == id)
                .Select(x=>x.AccountId)
                .FirstOrDefault();


            var star = _context.TaskLists
                   .Where(a => a.AccountId == accountid)
                  .SelectMany(x => x.ExpertApplications)
                  
                  .Select(b => new
                  {
                      評論 = b.Rating.RatingContent,
                      分數 = b.Rating.RatingStar,
                      評論日 = b.Rating.RatingDate,
                  });
               
            return Json(star);

        }

       }
}
