﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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
            //_memberID = 66;//登入者我自己memberID
        }

        // GET: Ratings
        public async Task<IActionResult> Index()
        {
            return View();


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
        public IActionResult CreateNew(int? id)//caseid
        {
            _memberID = GetMemberIDFromSession();//登入者我自己memberID
            if (_memberID == 0)
            {
                TempData["message"] = "請先登入";
                return RedirectToAction("Login", "Member");
            }

            CExperTaskFactory factory = new CExperTaskFactory(_context);
            CRatingCreatViewModel vm = new CRatingCreatViewModel();
            vm.taskid = id.GetValueOrDefault(); ;
            int memid = _context.TaskLists
                     .Where(x => x.CaseId == id)
                     .SelectMany(u => u.ExpertApplications)  // 使用SelectMany將集合“展開”
                     .Select(ea => ea.AccountId)
                .FirstOrDefault()
                .GetValueOrDefault();

            vm.委託者 = factory.MemberName(memid);
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
            CExperTaskFactory factory = new CExperTaskFactory(_context);
            int sourceid = factory.MemberID(vm.專家).GetValueOrDefault();
            int targetid = factory.MemberID(vm.委託者).GetValueOrDefault();
           
                DateTime date = DateTime.Now;
                Rating rating = new Rating();
                rating.RatingStar = vm.評論星數;
                rating.RatingContent = vm.評論內容;
                rating.RatingDate = date;
                rating.SourceRoleId = 3;
                rating.SourceAccountId = sourceid;
                rating.TargetRoleId = 1;
                rating.TargetAccountId = targetid;

                _context.Add(rating); 
               _context.SaveChanges();
                int newratingid= rating.RatingId;
                
           //EA表
                ExpertApplication expertapplication = _context.ExpertApplications
                    .Where(x=>x.CaseId==vm.taskid)
                    .FirstOrDefault();

            expertapplication.RatingId = newratingid;
            expertapplication.CaseStatusId = 12;//專家案件已評價
            _context.Update(expertapplication);
            _context.SaveChanges();

            return RedirectToAction("Listnew","ExpertTask");
          


        }

    }
}
