using Microsoft.AspNetCore.Mvc;
using prjCoreWantMember.ViewModels;
using prjCoreWebWantWant.Models;

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
            _memberID = 5; //假資料要刪喔

            // int memberID ;           

            //等會員ID可以傳遞再打開，sessionKey要跟SET的時候依樣
            //if (HttpContext.Session.Keys.Contains(sessionKey))
            //{
            //    memberID = HttpContext.Session.GetString(sessionKey);

            //}
            //   _memberID = memberID;
        }

        //星星評價API
        public IActionResult StarRatingAPI(int resumesid)
        {
            int accountid = _context.Resumes
                .Where(x => x.ResumeId == resumesid)
                .Select(x => x.AccountId)
                .FirstOrDefault();


            var star = _context.TaskLists
                   .Where(a => a.AccountId == accountid)
                  .SelectMany(x => x.ExpertApplications)

                  .Select(b => new
                  {
                      評論 = b.Rating.RatingContent,
                      分數 = b.Rating.RatingStar,
                      評論日 = b.Rating.RatingDate,
                      筆數 = b.Rating.RatingStar.Count()
                  });

            return Json(star);

        }
        //作品集評價
        public IActionResult WorkAPI(int resumesid)
        {
            int accountid = _context.Resumes
                .Where(x => x.ResumeId == resumesid)
                .Select(x => x.AccountId)
                .FirstOrDefault();


            var star = _context.TaskLists
                   .Where(a => a.AccountId == accountid)
                  .SelectMany(x => x.ExpertApplications)

                  .Select(b => new
                  {
                      評論 = b.Rating.RatingContent,
                      分數 = b.Rating.RatingStar,
                      評論日 = b.Rating.RatingDate,
                      筆數 = b.Rating.RatingStar.Count()
                  });

            return Json(star);

        }
    }
}
