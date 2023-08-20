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
            IEnumerable<MemberAccount> datas = null;
            if (string.IsNullOrEmpty(vm.txtKeyword))
                datas = from m in db.MemberAccounts
                        join l in db.LoginHistories on m.AccountId equals l.AccountId
                        join ms in db.MemberStatusLists on m.AccountStatus equals ms.AccountId
                        select new MemberAccount
                        {
                            AccountId = m.AccountId,
                            Name = m.Name,
                            Email = m.Email,
                            AccountStatus = m.AccountStatus,
                        };
            //else
            //    datas = db.ServiceContacts.Where(s => s.Account.Name.ToUpper().Contains(vm.txtKeyword.ToUpper())
            //        || s.Account.PhoneNo.ToUpper().Contains(vm.txtKeyword.ToUpper())
            //        || s.Account.Email.ToUpper().Contains(vm.txtKeyword.ToUpper()));

            return View(datas);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
