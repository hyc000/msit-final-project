using Microsoft.AspNetCore.Mvc;
using prjCoreWebWantWant.ViewModels;

namespace prjCoreWebWantWant.Controllers
{
    public class ForumApiController : Controller
    {
        [HttpPost]
        public IActionResult PostReply()
        {
            ForumPostReply reply = new ForumPostReply();


            return View();
        }
    }
}
