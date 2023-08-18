using Microsoft.AspNetCore.Mvc;
using prjCoreWebWantWant.Models;
using prjCoreWebWantWant.ViewModels;
using prjCoreWantMember.ViewModels;

namespace prjCoreWebWantWant.Controllers
{
    public class BackstageManagementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
