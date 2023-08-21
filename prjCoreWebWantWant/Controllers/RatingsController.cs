using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using prjCoreWebWantWant.Models;
using prjCoreWebWantWant.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace prjCoreWebWantWant.Controllers
{
    public class RatingsController : Controller
    {
        private readonly NewIspanProjectContext _context;
        private List<CRatings> _ratings ;
        private int _memberID;
        public RatingsController(NewIspanProjectContext context)
        {
            _context = context;
            _ratings = new List<CRatings>();
            _memberID = 17;//登入者我自己memberID
        }

        // GET: Ratings
        public async Task<IActionResult> Index()
        {
            CRatingViewModel vm =new CRatingViewModel();
            List<CRatings> ratingsForOther = new List<CRatings>();
            CRatings data=new CRatings();

            if (_context.Ratings != null) {
                //給別人的評論
                var ratingdata = await _context.Ratings
                    .Where(x => x.SourceAccountId == _memberID)
                    .Select(u => u)
                    .ToListAsync();



                foreach(var item in ratingdata)
                {
                    data.被評論者 = _context.MemberAccounts
                        .Where(x => x.AccountId == item.TargetAccountId)
                        .Select(u => u.Name)
                        .FirstOrDefault();

                    data.評論者 = "自己";
                    data.評論星數 = item.RatingStar;
                    data.評論內容 = item.RatingContent;
                    data.評論日期 = item.RatingDate;
                    ratingsForOther.Add(data);
                };
                vm.ForOtherRatings = ratingsForOther;


                List<CRatings> MyRatings = new List<CRatings>();
                CRatings datamy = new CRatings();

            
                //自己收到評論
                var ratingdatamy = await _context.Ratings
                    .Where(x => x.TargetAccountId == _memberID)
                    .Select(u => u)
                    .ToListAsync();
                foreach (var item in ratingdatamy)
                {
                    datamy.被評論者 = "自己";

                    datamy.評論者 =  _context.MemberAccounts
                        .Where(x => x.AccountId == item.SourceAccountId)
                        .Select(u => u.Name)
                        .FirstOrDefault();
                    datamy.評論星數 = item.RatingStar;
                    datamy.評論內容 = item.RatingContent;
                    datamy.評論日期 = item.RatingDate;
                    MyRatings.Add(datamy);
                };
                vm.MyRatings = MyRatings;

                return View(vm);
            }
            else
            {
                return Problem("Entity set 'NewIspanProjectContext.Ratings'  is null.");
            }


        }
      
        // GET: Ratings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Ratings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RatingId,RatingStar,RatingContent,RatingDate,SourceRoleId,SourceAccountId,TargetRoleId,TargetAccountId")] Rating rating)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rating);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rating);
        }



        public async Task<IActionResult> Ratingdata()
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
                    datarating.被評論者 = _context.MemberAccounts
                        .Where(x => x.AccountId == item.TargetAccountId)
                        .Select(u => u.Name)
                        .FirstOrDefault();

                    datarating.評論者 = "自己";
                    datarating.評論星數 = item.RatingStar;
                    datarating.評論內容 = item.RatingContent;
                    datarating.評論日期 = item.RatingDate;
                    ratingsForOther.Add(datarating);
                };
                 ;
                var data = ratingsForOther;
                return Json(data);

                //List<CRatings> MyRatings = new List<CRatings>();
                //CRatings datamy = new CRatings();


                ////自己收到評論
                //var ratingdatamy = await _context.Ratings
                //    .Where(x => x.TargetAccountId == _memberID)
                //    .Select(u => u)
                //    .ToListAsync();
                //foreach (var item in ratingdatamy)
                //{
                //    datamy.被評論者 = "自己";

                //    datamy.評論者 = _context.MemberAccounts
                //        .Where(x => x.AccountId == item.SourceAccountId)
                //        .Select(u => u.Name)
                //        .FirstOrDefault();
                //    datamy.評論星數 = item.RatingStar;
                //    datamy.評論內容 = item.RatingContent;
                //    datamy.評論日期 = item.RatingDate;
                //    MyRatings.Add(datamy);
                //};
                //vm.MyRatings = MyRatings;
                //var data = vm.ForOtherRatings;
                //return Json(data);
            }
            else
            {
                return Problem("Entity set 'NewIspanProjectContext.Ratings'  is null.");
            }


        }


        public async Task<IActionResult> RatingList()
        {
            return View();
        }

    }
}
