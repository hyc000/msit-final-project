using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjCoreWebWantWant.Models;

namespace prjShop.Controllers
{
    public class ShopController : Controller
    {
        private readonly NewIspanProjectContext _context;
        private readonly IWebHostEnvironment _host = null;

        public ShopController(NewIspanProjectContext context, IWebHostEnvironment host)
        {
            _context = context;
            _host = host;
        }

        public IActionResult ExpertShop()
        {
            var q = _context.Products
                .Include(t => t.Category)
                .Where(p => p.Status == "上架" && p.CategoryId == 1);

            return View(q);
        }
        public IActionResult CaseShop()
        {
            var q = _context.Products
              .Include(t => t.Category)
              .Where(p => p.Status == "上架" && p.CategoryId == 2);
            return View(q);
        }
        public IActionResult CheckOut()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Order()
        {
            return View();
        }
    }
}
