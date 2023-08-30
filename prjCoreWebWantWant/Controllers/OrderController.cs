using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using prjCoreWebWantWant.Models;
using prjCoreWebWantWant.ViewModels;
using System.Drawing.Printing;
using PagedList;
using PagedList.Mvc;

namespace prjShop.Controllers
{
    public class OrderController : Controller
    {
        private readonly NewIspanProjectContext _context;

        public OrderController(NewIspanProjectContext context)
        {
            _context = context;
        }
        public IActionResult List(int? categorys, DateTime? startDate, DateTime? endDate, int? orderId, string? name, int page = 1)
        {
            var query = _context.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .Include(o => o.Account)
                .Include(t => t.Category)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Resume)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Case)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                    .OrderByDescending(o => o.CreateTime);



            // 应用筛选条件
            if (categorys != null)
            {
                query = (IOrderedQueryable<Order>)query.Where(o => o.CategoryId == categorys);
            }

            if (startDate != null)
            {
                query = (IOrderedQueryable<Order>)query.Where(o => o.CreateTime >= startDate);
            }

            if (endDate != null)
            {
                query = (IOrderedQueryable<Order>)query.Where(o => o.CreateTime <= endDate);
            }

            if (orderId != null)
            {
                query = (IOrderedQueryable<Order>)query.Where(o => o.OrderId == orderId);
            }

            if (!string.IsNullOrEmpty(name))
            {
                query = (IOrderedQueryable<Order>)query.Where(o => o.Account.Name.Contains(name));
            }

            int pageSize = 10;
            var filteredOrders = query.ToList();

            // 注意这里的修改
            var pagedOrders = new PagedList<Order>(filteredOrders, page, pageSize);

            return View(pagedOrders);
        }
    }
}










