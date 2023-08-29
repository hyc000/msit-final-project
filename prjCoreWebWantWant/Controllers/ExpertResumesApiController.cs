using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjCoreWantMember.ViewModels;
using prjCoreWebWantWant.Models;
using System.Text.Json;

namespace prjCoreWebWantWant.Controllers
{
    public class ExpertResumesApiController : Controller
    {
        private readonly NewIspanProjectContext _context;
        private int _memberID;
        private CExpertInfoViewModel _cexpertresume;
        private List<ExpertWork> _work;
        public ExpertResumesApiController(NewIspanProjectContext context)
        {
            _context = context;
            _cexpertresume = new CExpertInfoViewModel();
            _work = new List<ExpertWork>();

            //Session取得
            //  _memberID = 5; //假資料要刪喔


        }
        //private int GetMemberIDFromSession()
        //{
        //    if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
        //    {
        //        string userDataJson = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
        //        MemberAccount loggedInUser = JsonSerializer.Deserialize<MemberAccount>(userDataJson);
        //        return loggedInUser.AccountId;
        //    }
        //    return 0;
        //}





        //星星評價API
        public IActionResult StarRatingAPI(int? resumesid)
        {
            if (!resumesid.HasValue || resumesid.Value <= 0)
            {
                return Json(new { Status = "Failed", Message = "無相關評分" });
            }

            CExperTaskFactory factory = new CExperTaskFactory(_context);
            //抓專家履歷中的會員ID
            int accountid = _context.Resumes
                .Where(x => x.ResumeId == resumesid)
                .Select(x => x.AccountId)
                .FirstOrDefault();

            //用專家會員ID去抓
            var star = _context.TaskLists
                   .Where(a => a.AccountId == accountid && a.ExpertApplications.Any(b => b.Rating != null))

                  .SelectMany(x => x.ExpertApplications)

                  .Select(b => new
                  {
                      評論者名字 = factory.MemberName(b.AccountId),
                      評論 = b.Rating.RatingContent,
                      分數 = b.Rating.RatingStar,
                      評論日 = b.Rating.RatingDate,

                  });

            var 筆數 = star.Count();

            var result = new
            {
                評分資料 = star,
                總筆數 = 筆數
            };

            return Json(result);

        }

        //履歷

        public IActionResult ResumeAPI(int? resumesid)
        {
            if (!resumesid.HasValue || resumesid.Value <= 0)
            {
                return Json(new { Status = "Failed", Message = "無相關履歷" });
            }

            var qexresume = _context.ExpertResumes
                .Where(a => a.ResumeId == resumesid)
                .FirstOrDefault();
            if (qexresume == null)
            {
                return Json(new { Status = "Failed", Message = "無相關介紹" });
            }

            return Json(qexresume);

        }

        //===================
        //專家履歷

        public IActionResult ExresumeAPI(int? resumesid)
        {
            if (!resumesid.HasValue || resumesid.Value <= 0)
            {
                return Json(new { Status = "Failed", Message = "無相關履歷" });
            }
            var qexresume = _context.ExpertResumes
                    .Where(a => a.ResumeId == resumesid)
                    .FirstOrDefault();
            if (qexresume == null)
            {
                return Json(new { Status = "Failed", Message = "無相關介紹" });
            }

            return Json(qexresume);
        }


        //作品集評價
        public IActionResult WorkAPI(int? resumesid)
        {
            if (!resumesid.HasValue || resumesid.Value <= 0)
            {
                return Json(new { Status = "Failed", Message = "無相關履歷" });
            }

            var qwork = _context.ExpertWorks
                    .Where(a => a.ExpertWorkLists.Any(x => x.ResumeId == resumesid))
                    .Select(y => new
                    {
                        作品名 = y.WorkTitle,
                        作品照片 = y.WorksPhoto,
                        作品日期 = y.DataCreateDate
                    });
            if (!qwork.Any())
            {
                return Json(new { Status = "Failed", Message = "無相關作品" });
            }

            return Json(qwork);




        }
    }
}
