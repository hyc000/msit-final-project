using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjCoreWebWantWant.Models;
using prjCoreWantMember.ViewModels;
using System.Diagnostics;
using prjCoreWebWantWant.ViewModels;

namespace prjCoreWebWantWant.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(CKeywordViewModel vm)
        {
            NewIspanProjectContext db = new NewIspanProjectContext();
            var viewModel = db.TaskLists.Select(taskList => new CIndexInfoViewModel
            {
                taskList = taskList,
                // 其他屬性的賦值
            }).FirstOrDefault();

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}