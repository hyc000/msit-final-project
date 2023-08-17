using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjCoreWebWantWant.Models;

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

            return View(q);
        }

        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Trash()
        {
            var q = _context.Products
                .Include(t => t.Category)
                .Where(p => p.Status == "垃圾桶");

            return View(q);
        }

        public IActionResult Edit()
        {
            return View();
        }
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
    }
}