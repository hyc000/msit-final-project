using Microsoft.AspNetCore.Mvc;
using prjCoreWebWantWant.Models;
using prjCoreWebWantWant.ViewModels;
using prjCoreWantMember.ViewModels;

namespace prjCoreWebWantWant.Controllers
{
    public class BackstageManagementController : Controller
    {
        //public IActionResult List(CKeywordViewModel vm)
        //{
        //    NewIspanProjectContext db = new NewIspanProjectContext();
        //    IEnumerable<MemberStatusList> datas = null;
        //    if (string.IsNullOrEmpty(vm.txtKeyword))
        //        datas = from m in db.MemberAccounts.FirstOrDefault().MemberStatusLists
        //                select m;
        //    //else
        //    //    datas = db.MemberAccounts.Where(m => m.Name.ToUpper().Contains(vm.txtKeyword.ToUpper()));
        //    return View(datas);
        //}

        public IActionResult Index()
        {
            return View();
        }
    }
}
