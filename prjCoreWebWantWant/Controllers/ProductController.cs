using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public IActionResult List()
        {
            var q = _context.Products
                .Include(t => t.Category)
                .Where(p => p.Status == "上架" || p.Status == "下架");
            ViewBag.Categories = _context.Categories.ToList();
            return View(q);
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
            return RedirectToAction("List");
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
                }
            }
            return RedirectToAction("List");
        }
        //改狀態
        [HttpPost]
        public IActionResult ChangeStatus(int id, string status)
        {
            var prod = _context.Products.FirstOrDefault(p => p.ProductId == id);
            if (prod != null)
            {
                prod.Status = status;
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
        //真的刪除
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
    }
}