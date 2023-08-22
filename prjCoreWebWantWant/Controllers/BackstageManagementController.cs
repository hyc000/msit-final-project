using Microsoft.AspNetCore.Mvc;
using prjCoreWebWantWant.Models;
using prjCoreWebWantWant.ViewModels;
using prjCoreWantMember.ViewModels;

namespace prjCoreWebWantWant.Controllers
{
    public class BackstageManagementController : Controller
    {
        private readonly NewIspanProjectContext _context;

        public BackstageManagementController(NewIspanProjectContext context)
        {
            _context = context;
        }

        public IActionResult List(CKeywordViewModel vm)
        {
            NewIspanProjectContext db = new NewIspanProjectContext();
            IEnumerable<CBackstageManagementViewModel> datas = null;
            if (string.IsNullOrEmpty(vm.txtKeyword))
                datas = from ms in db.MemberStatusLists
                        join m in db.MemberAccounts on ms.AccountId equals m.AccountId
                        join l in db.LoginHistories on ms.AccountId equals l.AccountId
                        let lastLoginTime = (from ms in db.MemberStatusLists
                                             where ms.AccountId == m.AccountId
                                             orderby ms.UpdateTime descending
                                             select ms.UpdateTime).FirstOrDefault()
                        select new CBackstageManagementViewModel
                        {
                            memberStatusList = ms,
                            memberAccount = m,
                            loginHistory = l,
                        };
            else
                datas = from ms in db.MemberStatusLists
                        join m in db.MemberAccounts
                        on ms.AccountId equals m.AccountId
                        where m.Name.Contains(vm.txtKeyword) ||
                        m.Email.Contains(vm.txtKeyword) ||
                        m.PhoneNo.Contains(vm.txtKeyword)
                        select new CBackstageManagementViewModel
                        {
                            memberStatusList = ms,
                            memberAccount = m
                        };

            return View(datas);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
