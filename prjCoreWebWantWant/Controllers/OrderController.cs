using Microsoft.AspNetCore.Mvc;

namespace prjShop.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult List()
        {
            return View();
        }
    }
}
