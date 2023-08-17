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

        public ExpertResumesController(NewIspanProjectContext context)
        {
            _context = context;
        }

        // 這個到時候可以刪掉
        public async Task<IActionResult> Index()
        {
            var newIspanProjectContext = _context.ExpertResumes.Include(e => e.Resume);
            return View(await newIspanProjectContext.ToListAsync());
        }

        // GET: ExpertResumes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            CExpertInfoViewModel cexpertresume = new CExpertInfoViewModel();
            if (cexpertresume == null)
            {
                return NotFound();
            }


            if (id == null || _context.ExpertResumes == null)
            {
                return NotFound();
            }

            cexpertresume.expertResume = await _context.ExpertResumes
                .Include(e => e.Resume)
                .FirstOrDefaultAsync(m => m.ResumeId == id);
            if (_context.ExpertWorkLists != null)
            {
                cexpertresume.expertWorkList = await _context.ExpertWorkLists
                 .Include(e => e.Resume)
                 .FirstOrDefaultAsync(m => m.ResumeId == id);
            }
            if (_context.Resumes != null)
            {
                cexpertresume.resume = await _context.Resumes
                 .Where(a => a.ResumeId == id)
                 .FirstOrDefaultAsync();
            }
            if (_context.MemberAccounts != null)
            {


            }


            return View(cexpertresume);
        }

       
    }
}
