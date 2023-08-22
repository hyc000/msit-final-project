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
            return View();


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



        //不用了改回INDEX

        public async Task<IActionResult> RatingList()
        {
            return View();
        }

    }
}
