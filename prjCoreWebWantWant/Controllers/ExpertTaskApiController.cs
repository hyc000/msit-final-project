using Microsoft.AspNetCore.Mvc;
using prjCoreWantMember.ViewModels;
using prjCoreWebWantWant.Models;
using prjCoreWebWantWant.ViewModels;

namespace prjCoreWebWantWant.Controllers
{
    [Route("ExpertTaskApi")]
    [ApiController]
    public class ExpertTaskApiController : Controller
    {
        private readonly NewIspanProjectContext _context;
     
        private readonly IWebHostEnvironment _env;
        public ExpertTaskApiController(NewIspanProjectContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;

        }
        //委託修改評價
        //
     
    }

}

    

 
