using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjCoreWebWantWant.Models;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

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
            // 依據篩選條件撈取訂單List
            var query = getFilteredOrders(categorys, startDate, endDate, orderId, name, purchaseTime);
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

        public IActionResult Card() 
        {
            return View();
        }

        public IActionResult exportExcel(int? categorys, DateTime? startDate, DateTime? endDate, int? orderId, string? name, string? purchaseTime)
        {
            // 依據篩選條件撈取訂單List
            var query = getFilteredOrders(categorys, startDate, endDate, orderId, name, purchaseTime);
            var filteredOrders = query.ToList();

            // 建立Excel
            HSSFWorkbook workbook = new HSSFWorkbook(); // 建立活頁簿
            ISheet sheet = workbook.CreateSheet("sheet"); // 建立分頁

            // 設定欄寬
            sheet.SetColumnWidth(2, 40 * 256);
            sheet.SetColumnWidth(3, 30 * 256);
            sheet.SetColumnWidth(9, 20 * 256);

            // 設定樣式
            ICellStyle style = workbook.CreateCellStyle();
            IFont font = workbook.CreateFont();
            style.Alignment = HorizontalAlignment.Center; // 水平置中
            style.VerticalAlignment = VerticalAlignment.Center; // 垂直置中
            style.WrapText = true; // 自動換行
            style.BorderBottom = BorderStyle.Thin;
            style.BorderTop = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            font.FontName = "微軟正黑體";
            style.SetFont(font);

            // 新增標題
            String header = "訂單編號,訂單類別,案件,購買時間,會員,付款狀態,商品金額,使用紅利,訂單金額,商品名稱,數量,單價,小計";
            String[] headerArr = header.Split(",");
            var headerRow = sheet.CreateRow(0); // 開出一列
            for (int i = 0 ; i < headerArr.Length ; i++) // 開欄位塞值
            {
                var cell = headerRow.CreateCell(i);
                cell.SetCellValue(headerArr[i]);
            }

            // 新增內容
            int rowIndex = 1;
            foreach (var order in filteredOrders)
            {
                foreach (var orderDetail in order.OrderDetails)
                {
                    // 開出一列
                    var dataRow = sheet.CreateRow(rowIndex);

                    // 開欄位塞值
                    dataRow.CreateCell(0).SetCellValue(order.OrderId); // 訂單編號
                    dataRow.CreateCell(1).SetCellValue(order.Category.CategoryName); // 訂單類別
                    // 案件
                    if (orderDetail.CaseId != null)
                    {
                        dataRow.CreateCell(2).SetCellValue(orderDetail.Case.TaskTitle);
                    }
                    else if (orderDetail.ResumeId != null)
                    {
                        dataRow.CreateCell(2).SetCellValue(orderDetail.Resume.ResumeTitle);
                    }
                    dataRow.CreateCell(3).SetCellValue(order.CreateTime.ToString()); // 購買時間
                    dataRow.CreateCell(4).SetCellValue(order.Account.Name); // 會員
                    dataRow.CreateCell(5).SetCellValue(order.Status); // 付款狀態
                    dataRow.CreateCell(6).SetCellValue((double) order.OrderPrice + " 元"); // 商品金額
                    dataRow.CreateCell(7).SetCellValue((double) order.OrderUsePoint + " 點"); // 使用紅利
                    dataRow.CreateCell(8).SetCellValue((double) order.PaidAmount + " 元"); // 訂單金額
                    dataRow.CreateCell(9).SetCellValue(orderDetail.Product.ProductName); // 商品名稱
                    dataRow.CreateCell(10).SetCellValue((double) orderDetail.Quantity); // 數量
                    dataRow.CreateCell(11).SetCellValue((double) orderDetail.UnitPrice + " 元"); // 單價
                    dataRow.CreateCell(12).SetCellValue((double) (orderDetail.Quantity * orderDetail.UnitPrice) + " 元"); // 小計

                    rowIndex++;
                }
            }

            // 套用樣式
            for (int row = 0; row <= sheet.LastRowNum; row++)
            {
                var dataRow = sheet.GetRow(row);
                if (dataRow != null)
                {
                    for (int i = 0; i < headerArr.Length; i++)
                    {
                        var cell = dataRow.GetCell(i);
                        if (cell != null)
                        {
                            cell.CellStyle = style;
                        }
                    }
                }
            }

            // 寫入Excel內容到MemoryStream
            var excelData = new MemoryStream();
            workbook.Write(excelData);

            // 檔案名稱
            string formattedDateTime = DateTime.Now.ToString("yyyyMMddhhmmss");
            string fileName = $"訂單明細_{formattedDateTime}.xls";

            return File(excelData.ToArray(), "application/vnd.ms-excel", string.Format(fileName));
        }

        // 【共用方法】依據篩選條件撈取訂單query
        private IQueryable<Order> getFilteredOrders(int? categorys, DateTime? startDate, DateTime? endDate, int? orderId, string? name, string? purchaseTime)
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
                // 固定排序方式
                query = query.OrderByDescending(o => o.CreateTime);
            }

            return query;
        }

    }
}