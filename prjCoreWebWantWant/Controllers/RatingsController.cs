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
            _memberID = 66;//登入者我自己memberID
        }

        // GET: Ratings
        public async Task<IActionResult> Index()
        {
            return View();


        }
        public IActionResult CreateNew(int? id)//caseid
        {
            CExperTaskFactory factory = new CExperTaskFactory(_context);
            CRatingCreatViewModel vm = new CRatingCreatViewModel();
            vm.taskid = id.GetValueOrDefault(); ;
            vm.委託者 = factory.MemberName(_memberID);
           int expertid= _context.TaskLists
                .Where(x => x.CaseId == id)
                .Select(u => u.AccountId)
                .FirstOrDefault()
                .GetValueOrDefault(); 
            vm.專家 = factory.MemberName(expertid);
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateNew([Bind("taskid,委託者,專家,評論星數,評論內容")] CRatingCreatViewModel vm)
        {
            //if (ModelState.IsValid) 
            //{
                CExperTaskFactory factory = new CExperTaskFactory(_context);

                DateTime date = DateTime.Now;
                Rating rating = new Rating();
                rating.RatingStar = vm.評論星數;
                rating.RatingContent = vm.評論內容;
                rating.RatingDate = date;
                rating.SourceRoleId = 3;
                rating.SourceAccountId = factory.MemberID(vm.專家);
                rating.TargetRoleId = 1;
                rating.TargetAccountId = factory.MemberID(vm.委託者);

                _context.Add(rating); 
               _context.SaveChanges();
                int newratingid= rating.RatingId;
                
                ExpertApplication expertapplication = _context.ExpertApplications
                    .Where(x=>x.CaseId==vm.taskid)
                    .FirstOrDefault();
                expertapplication.RatingId = newratingid;

                _context.Update(expertapplication);
                _context.SaveChanges();


                return RedirectToAction("Index");
            //}
            //return View();


        }


        // GET: Ratings/Create
        //    public IActionResult Create(int? id)
        //{
        //CRatingCreatViewModel vm=new CRatingCreatViewModel();
        //vm.taskprincipal = _context.MemberAccounts
        //    .Where(x => x.AccountId == _memberID)
        //    .Select(u => u.Name)
        //    .FirstOrDefault();
        //vm.taskexperter = _context.MemberAccounts
        //    .Where(x => x.AccountId == id)
        //    .Select(u => u.Name)
        //    .FirstOrDefault();


        //return View(vm);
        //}

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



        //不用了改回INDEX

        //public async Task<IActionResult> RatingList()
        //{
        //    return View();
        //}

    }
}
