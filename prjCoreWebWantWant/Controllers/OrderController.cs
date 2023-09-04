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
using DocumentFormat.OpenXml.InkML;
using System.Globalization;

namespace prjShop.Controllers
{
    public class OrderController : Controller
    {
        private readonly NewIspanProjectContext _context;

        public OrderController(NewIspanProjectContext context)
        {
            _context = context;
        }
        public IActionResult List(int? categorys, DateTime? startDate, DateTime? endDate, int? orderId, string? name, string? purchaseTime)
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


            // 篩選條件
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
            if (!string.IsNullOrEmpty(purchaseTime))
            {
                if (purchaseTime == "asc")
                {
                    query = query.OrderBy(o => o.CreateTime);
                }
                else if (purchaseTime == "desc")
                {
                    query = query.OrderByDescending(o => o.CreateTime);
                }
            }
            else
            {
                // 默认排序方式（可根据需求设置）
                query = query.OrderByDescending(o => o.CreateTime);
            }

            var filteredOrders = query.ToList();


            // 計算訂單數量和總金額
            int orderCount = query.Count();
            decimal totalAmount = query.Sum(o => (int)o.PaidAmount);

            ViewBag.OrderCount = orderCount;
            ViewBag.TotalAmount = totalAmount;

            return View(filteredOrders);
        }

        public IActionResult ShoppingCharts()
        {
            /* --------------- 上架中的商品類型銷售數量---------------  */
            var query1 = from od in _context.OrderDetails
                        join p in _context.Products on od.ProductId equals p.ProductId
                        //where p.Status == "上架"
                        group od by p.CategoryId into g
                        select new 
                        {
                            CategoryID = g.Key,
                            TotalQuantity = g.Sum(od => od.Quantity)
                        };

            // 將查詢結果存儲在ViewBag中
            var chartData1 = query1.ToList();
            ViewBag.ChartData1 = chartData1;



            /* --------------- 每日總銷售數量---------------  */
            var query2 = _context.OrderDetails
                        .Join(_context.Orders,
                            od => od.OrderId,
                            o => o.OrderId,
                            (od, o) => new { od.Quantity, o.CreateTime })
                        .Where(x => x.CreateTime.HasValue) // 確保 CreateTime 不為空
                        .GroupBy(x => x.CreateTime.Value.Date) // 使用 .Value.Date 將 DateTime? 轉換為 Date
                        .Select(x => new { Date = x.Key, TotalQuantity = x.Sum(y => y.Quantity) });

            // 將查詢結果存儲在ViewBag中
            var chartData2 = query2.ToList();
            ViewBag.ChartData2 = chartData2;



            /* --------------- 前三大熱銷商品（長條圖）--------------- */
            var query3 = from od in _context.OrderDetails
                         group od by new
                         {
                             od.ProductId,
                             od.Product.ProductName
                         } into g
                         select new
                         {
                             ProductId = g.Key.ProductId,
                             ProductName = g.Key.ProductName,
                             TotalQuantity = g.Sum(od => od.Quantity)
                         };

            // 將查詢結果存儲在ViewBag中
            var chartData3 = query3.OrderByDescending(item => item.TotalQuantity).Take(3).ToList();
            ViewBag.ChartData3 = chartData3;

            return View();
        }
    }
}