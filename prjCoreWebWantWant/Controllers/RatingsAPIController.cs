using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjCoreWebWantWant.Models;
using prjCoreWebWantWant.ViewModels;

namespace prjCoreWebWantWant.Controllers
{
    public class RatingsAPIController : Controller
    {
        private readonly NewIspanProjectContext _context;
        private List<CRatings> _ratings;
        private int _memberID;
        public RatingsAPIController(NewIspanProjectContext context)
        {
            _context = context;
            _ratings = new List<CRatings>();
            _memberID = 17;//登入者我自己memberID
        }

        public async Task<IActionResult> ForOtherdata()
        {

            List<CRatings> ratingsForOther = new List<CRatings>();
            CRatings datarating = new CRatings();

            if (_context.Ratings != null)
            {
                //給別人的評論
                var ratingdata = await _context.Ratings
                    .Where(x => x.SourceAccountId == _memberID)
                    .Select(u => u)
                    .ToListAsync();



                foreach (var item in ratingdata)
                {
                    datarating.ratedperson = _context.MemberAccounts
                        .Where(x => x.AccountId == item.TargetAccountId)
                        .Select(u => u.Name)
                        .FirstOrDefault();

                    datarating.ratingforperson = "自己";
                    datarating.ratingstar = item.RatingStar;
                    datarating.ratingcontent = item.RatingContent;
                    datarating.ratingdate = item.RatingDate;
                    ratingsForOther.Add(datarating);

                };
                
                var data = ratingsForOther;
                return Json(data);

            }
            else
            {
                return Problem("Entity set 'NewIspanProjectContext.Ratings'  is null.");
            }


        }


        public async Task<IActionResult> MyRatingsdata()
        {
            List<CRatings> MyRatings = new List<CRatings>();
            CRatings datamy = new CRatings();

            if (_context.Ratings != null)
            {
               
                //自己收到評論
                var ratingdatamy = await _context.Ratings
                    .Where(x => x.TargetAccountId == _memberID)
                    .Select(u => u)
                    .ToListAsync();
                foreach (var item in ratingdatamy)
                {
                    datamy.ratedperson = "自己";

                    datamy.ratingforperson = _context.MemberAccounts
                        .Where(x => x.AccountId == item.SourceAccountId)
                        .Select(u => u.Name)
                        .FirstOrDefault();
                    datamy.ratingstar = item.RatingStar;
                    datamy.ratingcontent = item.RatingContent;
                    datamy.ratingdate = item.RatingDate;
                    MyRatings.Add(datamy);
                };

                var data = MyRatings;

                return Json(data);

               


            }
            else
            {
                return Problem("Entity set 'NewIspanProjectContext.Ratings'  is null.");
            }


        }










    }
}
