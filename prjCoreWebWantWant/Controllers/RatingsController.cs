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
                    data.ratedperson = _context.MemberAccounts
                        .Where(x => x.AccountId == item.TargetAccountId)
                        .Select(u => u.Name)
                        .FirstOrDefault();

                    data.ratingforperson = "自己";
                    data.ratingstar = item.RatingStar;
                    data.ratingcontent = item.RatingContent;
                    data.ratingdate = item.RatingDate;
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
                    datamy.ratedperson = "自己";

                    datamy.ratingforperson =  _context.MemberAccounts
                        .Where(x => x.AccountId == item.SourceAccountId)
                        .Select(u => u.Name)
                        .FirstOrDefault();
                    datamy.ratingstar = item.RatingStar;
                    datamy.ratingcontent = item.RatingContent;
                    datamy.ratingdate = item.RatingDate;
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
        public IActionResult Create(int? id)
        {
            CRatingCreatViewModel vm=new CRatingCreatViewModel();
            vm.taskprincipal = _context.MemberAccounts
                .Where(x => x.AccountId == _memberID)
                .Select(u => u.Name)
                .FirstOrDefault();
            vm.taskexperter = _context.MemberAccounts
                .Where(x => x.AccountId == id)
                .Select(u => u.Name)
                .FirstOrDefault();


            return View(vm);
        }

        // POST: Ratings/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create()
        {
            //if (ModelState.IsValid) async
            //{
            //    _context.Add(rating); CRatingCreatViewModel vm
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //return View(rating);
            return View();
        }





        public async Task<IActionResult> RatingList()
        {
            return View();
        }

    }
}
