using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
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
        public IActionResult List(int? categorys, DateTime? startDate, DateTime? endDate, int? orderId, string? name)
        {
            var query = _context.Orders
          .Include(o => o.OrderDetails)
              .ThenInclude(od => od.Product)
          .Include(o => o.Account)
          .Include(o => o.Category)
          .AsQueryable();  //将查询初始化为可构建的Queryable

            if (categorys.HasValue)
            {
                query = query.Where(o => o.CategoryId == categorys);
            }

            if (startDate.HasValue)
            {
                query = query.Where(o => o.CreateTime >= startDate);
            }

            if (endDate.HasValue)
            {
                query = query.Where(o => o.CreateTime <= endDate);
            }

            if (orderId.HasValue)
            {
                query = query.Where(o => o.OrderId == orderId);
            }

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(o => o.Account.Name.Contains(name));
            }

            var filteredOrders = query.ToList();

            var cases = _context.TaskLists.ToList();
            var resumes = _context.Resumes
    .Include(r => r.ExpertResume)
    .ToList();

            foreach (var resume in resumes)
            {
                if (resume.ExpertResume == null)
                {
                    Console.WriteLine($"ExpertResume is null for ResumeId: {resume.ResumeId}");
                }
                else if (resume.ExpertResume.Introduction == null)
                {
                    Console.WriteLine($"Introduction is null for ResumeId: {resume.ResumeId}");
                }
            }

            ViewBag.Cases = cases.ToDictionary(c => c.CaseId, c => c.TaskTitle);
            ViewBag.Resumes = resumes.ToDictionary(r => r.ResumeId, r => r.ExpertResume?.Introduction ?? "No Introduction");

            return View(filteredOrders);
        }



    }
}

