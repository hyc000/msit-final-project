using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using prjCoreWebWantWant.Models;
using System.Reflection.Metadata.Ecma335;

namespace prjShop.Controllers
{
    public class ProductController : Controller


    {
        private readonly NewIspanProjectContext _context;
        private readonly IWebHostEnvironment _host = null;
        public ProductController(NewIspanProjectContext context, IWebHostEnvironment host)
        {
            _context = context;
            _host = host;
        }
  

        public IActionResult List(int? category, string status, DateTime? startDate, DateTime? endDate, string productName, decimal? minPrice, decimal? maxPrice)
        {
            var query = _context.Products.Include(t => t.Category).Where(p => p.Status == "上架" || p.Status == "下架");

            if (category != null)
            {
                query = query.Where(p => p.Category.CategoryId == category);
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(p => p.Status == status);
            }

            if (startDate != null)
            {
                query = query.Where(p => p.PostStartDate >= startDate);
            }

            if (endDate != null)
            {
                query = query.Where(p => p.PostStartDate <= endDate);
            }

            if (!string.IsNullOrEmpty(productName))
            {
                query = query.Where(p => p.ProductName.Contains(productName));
            }

            if (minPrice != null)
            {
                query = query.Where(p => p.UnitPrice >= minPrice);
            }

            if (maxPrice != null)
            {
                query = query.Where(p => p.UnitPrice <= maxPrice);
            }
            // 添加降序排序
            query = query.OrderByDescending(p => p.PostStartDate);

            ViewBag.Categories = _context.Categories.ToList();
            return View(query.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        //新增商品
        public IActionResult Create(Product p, IFormFile file)
        {

            string uniqueFileName = Guid.NewGuid().ToString().Substring(0, 8) + file.FileName;
            string filePath = Path.Combine(_host.WebRootPath, "shopimg", uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            p.PostStartDate = DateTime.Now;
            p.GetPoint = p.UnitPrice / 10;

            p.CoverPhoto = uniqueFileName; // 使用亂數名稱作為圖片名稱

            _context.Products.Add(p);
            _context.SaveChanges();

            TempData["SweetAlertMessage"] = "新增成功!";
            TempData["SweetAlertMessageType"] = "success";

            return View("Create");
        }
        [HttpPost]
        //批次新增
        public IActionResult batchImport(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return Json(new { success = false, message = "請選擇要上傳的檔案！" });
            }

            if (Path.GetExtension(file.FileName).ToLower() != ".xlsx")
            {
                return Json(new { success = false, message = "限上傳xlsx檔案！" });
            }

            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                stream.Position = 0;
                using (var package = new XSSFWorkbook(stream))
                {
                    ISheet sheet = package.GetSheetAt(0);
                    List<string> importErrors = new List<string>();

                    if (sheet.LastRowNum <= 0)
                    {
                        return Json(new { success = false, message = "檔案中沒有提供任何商品資料，請確認！" });
                    }

                    for (int rowIdx = 0; rowIdx <= sheet.LastRowNum; rowIdx++)
                    {
                        IRow row = sheet.GetRow(rowIdx);
                        if (row == null)
                        {
                            continue;
                        }

                        // 檢查每個欄位是否為空
                        bool isRowInvalid = false;
                        for (int cellIdx = 0; cellIdx < row.LastCellNum; cellIdx++)
                        {
                            ICell cell = row.GetCell(cellIdx);
                            if (cell == null || cell.CellType == CellType.Blank)
                            {
                                isRowInvalid = true; // 如果任何一個欄位為空，將該列標記為無效
                                break; // 停止檢查，因為已經確定該列無效
                            }
                        }
                        if (isRowInvalid)
                        {
                            importErrors.Add($"已完成前{rowIdx}列匯入，由於第{rowIdx + 1}列資料填寫不完整，無法繼續匯入！");
                            break;
                        }

                        try
                        {
                            Product p = new Product();
                            if (rowIdx == 0)
                            {
                                continue; // 跳過標題列
                            }

                            if ("專家曝光".Equals(row.GetCell(0).StringCellValue))
                            {
                                p.CategoryId = 1;
                            }
                            else if ("任務曝光".Equals(row.GetCell(0).StringCellValue))
                            {
                                p.CategoryId = 2;
                            }

                            p.ProductName = row.GetCell(1).StringCellValue;
                            p.TopType = row.GetCell(2).StringCellValue;
                            p.TopDate = (int)row.GetCell(3).NumericCellValue;
                            p.UnitPrice = (int)row.GetCell(4).NumericCellValue;
                            p.UnitsInStock = (int)row.GetCell(5).NumericCellValue;
                            p.ProductDesc = row.GetCell(6).StringCellValue;
                            p.Status = "下架";
                            p.PostStartDate = DateTime.Now;
                            p.GetPoint = p.UnitPrice / 10;

                            string filePath = Path.Combine(_host.WebRootPath, "shopimg", "noimage.jpg");
                            p.CoverPhoto = "noimage.jpg";

                            _context.Products.Add(p);
                            _context.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            importErrors.Add($"匯入第{rowIdx + 1}列時發生錯誤：{ex.Message}");
                        }
                    }

                    if (importErrors.Any())
                    {
                        return Json(new { success = false, message = string.Join("\n", importErrors) });
                    }

                    return Json(new { success = true, message = "商品已匯入成功！" });
                }
            }
        }
        //類別載入用
        public IActionResult Category()
        {
            var category = _context.Categories.Select(c => new
            {
                categoryId = c.CategoryId,
                categoryName = c.CategoryName
            }).ToList();

            return Json(category);
        }
        //垃圾桶頁
        public IActionResult Trash()
        {
            var q = _context.Products
                .Include(t => t.Category)
                .Where(p => p.Status == "垃圾桶");

            return View(q);
        }


        //移到垃圾車
        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                var prod = _context.Products.FirstOrDefault(p => p.ProductId == id);
                if (prod != null)
                {
                    prod.Status = "垃圾桶";
                    _context.SaveChanges();
                    return Json(new { success = true });
                }
            }
            return Json(new { success = false }); 
        }
        //改狀態
        [HttpPost]
        public IActionResult ChangeStatus(int id, string status)
        {
            var prod = _context.Products.FirstOrDefault(p => p.ProductId == id);
            if (prod != null)
            {
    
                    prod.Status = status; // 否則保持原本的狀態
          
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        //垃圾桶返回到列表
        public IActionResult GoBack(int? id)
        {
            if (id != null)
            {

                var prod = _context.Products.FirstOrDefault(p => p.ProductId == id);
                if (prod != null)
                {
                    prod.Status = "下架";
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("Trash");
        }
        //垃圾桶-刪除
        public IActionResult RealDelete(int? id)
        {
            if (id != null)
            {

                var prod = _context.Products.FirstOrDefault(p => p.ProductId == id);
                if (prod != null)
                {
                    prod.Status = "刪除";
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("Trash");
        }
        //垃圾桶刪除全部
        public IActionResult DeleteAll()
        {
            var productsInTrash = _context.Products.Where(p => p.Status == "垃圾桶").ToList();
            foreach (var prod in productsInTrash)
            {
                prod.Status = "刪除";
            }
            _context.SaveChanges();

            return RedirectToAction("Trash");
        }
        // 垃圾桶-選定商品復原
        [HttpPost]
        public IActionResult GoBackSelect(List<int> selectedIds)
        {
            var productsToRestore = _context.Products.Where(p => selectedIds.Contains(p.ProductId)).ToList();
            foreach (var prod in productsToRestore)
            {
                prod.Status = "下架";
            }
            _context.SaveChanges();

            return RedirectToAction("Trash");
        }
       
        // 垃圾桶-永久删除選定商品
        [HttpPost]
        public IActionResult DeleteSelected(List<int> selectedIds)
        {
            var productsToDelete = _context.Products.Where(p => selectedIds.Contains(p.ProductId)).ToList();
            foreach (var prod in productsToDelete)
            {
                prod.Status = "刪除";
            }
            _context.SaveChanges();

            return RedirectToAction("Trash");
        }

        //載入要修改資料到MODAL
        public IActionResult Edit(int id)
        {
            var product = _context.Products            
                .FirstOrDefault(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }
            return Json(product);
        }
        //送出修改資料
        [HttpPost]
        public IActionResult Edit(Product Pln, IFormFile file)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == Pln.ProductId);

            if (product == null)
            {
                return NotFound();
            }       
            // 更新封面
            if (file != null)
            {
              
                string uniqueFileName = Guid.NewGuid().ToString().Substring(0, 8) + file.FileName;            
                string filePath = Path.Combine(_host.WebRootPath, "shopimg", uniqueFileName);            
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
   
                product.CoverPhoto = uniqueFileName;
            }

            product.ProductDesc=Pln.ProductDesc;
            product.UnitPrice = Pln.UnitPrice;
            product.Status = Pln.Status;
            product.UnitsInStock = Pln.UnitsInStock;
            product.GetPoint = product.UnitPrice/10;

            _context.SaveChanges();

            return Json(new { success = true });
        }
        //匯出範例EXCEL
        public IActionResult DownloadTemplate()
        {
            var templatePath = Path.Combine(_host.WebRootPath, "Excel", "匯入曝光商品範本.xlsx");

            if (System.IO.File.Exists(templatePath))
            {
                return PhysicalFile(templatePath, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "匯入曝光商品範本.xlsx");
            }
            else
            {
                return NotFound(); // 如果文件不存在，返回404錯誤
            }
        }

    }
}