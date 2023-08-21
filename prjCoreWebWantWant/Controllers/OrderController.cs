using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjCoreWebWantWant.Models;

namespace prjShop.Controllers
{
    public class OrderController : Controller
    {
        private readonly NewIspanProjectContext _context;

        public OrderController(NewIspanProjectContext context) 
        {
            _context = context;
        }
        public IActionResult List()
        {
            var o = _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
             .Include(o => o.Account)
             .Include(o =>o.Category)
         
             .ToList();

            var cases = _context.TaskLists.ToList();
            var resumes = _context.Resumes.ToList();

            ViewBag.Cases = cases.ToDictionary(c => c.CaseId, c => c.TaskTitle);
            ViewBag.Resumes = resumes.ToDictionary(r => r.ResumeId, r => r.ResumeId);

            return View(o);
        }
    }
}
