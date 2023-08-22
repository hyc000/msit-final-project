using Microsoft.AspNetCore.Mvc;
using prjCoreWebWantWant.Models;
using prjCoreWebWantWant.ViewModels;
using prjCoreWantMember.ViewModels;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

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
                //datas = (from m in db.MemberAccounts
                //         join ms in db.MemberStatusLists on m.AccountId equals ms.AccountId
                //         join l in db.LoginHistories on m.AccountId equals l.AccountId
                //         let lastStatusChannge = (from ms in db.MemberStatusLists
                //                                  where ms.AccountId == m.AccountId
                //                                  orderby ms.StatusChangeId descending
                //                                  select ms).FirstOrDefault()
                //         let lastLoginTime = (from l in db.LoginHistories
                //                              where l.AccountId == m.AccountId
                //                              orderby l.LoginId descending
                //                              select l).FirstOrDefault()
                //         select new CBackstageManagementViewModel
                //         {
                //             memberAccount = m,
                //             memberStatusList = ms,
                //             loginHistory = lastLoginTime,
                //         }).AsEnumerable().DistinctBy(vm => vm.memberAccount.AccountId);
                datas = (from m in db.MemberAccounts
                         join ms in db.MemberStatusLists on m.AccountId equals ms.AccountId into msGroup
                         from ms in msGroup.DefaultIfEmpty() // 左外連接
                         join l in db.LoginHistories on m.AccountId equals l.AccountId into lGroup
                         from l in lGroup.DefaultIfEmpty() // 左外連接
                         let lastStatusChannge = (from ms in db.MemberStatusLists
                                                  where ms.AccountId == m.AccountId
                                                  orderby ms.StatusChangeId descending
                                                  select ms).FirstOrDefault()
                         let lastLoginTime = (from l in db.LoginHistories
                                              where l.AccountId == m.AccountId
                                              orderby l.LoginId descending
                                              select l).FirstOrDefault()
                         select new CBackstageManagementViewModel
                         {
                             memberAccount = m,
                             memberStatusList = ms,
                             loginHistory = lastLoginTime,
                         }).AsEnumerable().DistinctBy(vm => vm.memberAccount.AccountId);

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
