using Microsoft.AspNetCore.Mvc;
using prjCoreWebWantWant.Models;
using prjCoreWebWantWant.ViewModels;
using prjCoreWantMember.ViewModels;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using X.PagedList;
using Microsoft.EntityFrameworkCore;
using DocumentFormat.OpenXml.InkML;

namespace prjCoreWebWantWant.Controllers
{
    public class BackstageManagementController : Controller
    {
        private readonly NewIspanProjectContext _context;

        public BackstageManagementController(NewIspanProjectContext context)
        {
            _context = context;
        }

        public IActionResult Detail(int? id)
        {
            NewIspanProjectContext db = new NewIspanProjectContext();
            IEnumerable<CBackstageManagementViewModel> datas = null;

            datas = from m in db.MemberAccounts
                    join l in db.LoginHistories
                    on m.AccountId equals l.AccountId

                    join ms in db.MemberStatusLists
                    on m.AccountId equals ms.AccountId

                    select new CBackstageManagementViewModel
                    {

                    };

            return View(datas);

        }

        public IActionResult List(CKeywordViewModel vm,int currentPage = 1)
        {
            NewIspanProjectContext db = new NewIspanProjectContext();
            IEnumerable<CBackstageManagementViewModel> datas = null;

            int pageSize = 8;
            int skipAmount = (currentPage - 1) * pageSize;

            int totalCount = db.MemberAccounts.Count();
            int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            if (string.IsNullOrEmpty(vm.txtKeyword))

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
                             AccountId = m.AccountId,
                             Name = m.Name,
                             Email = m.Email,
                             PhoneNo = m.PhoneNo,
                             Gender = m.Gender,
                             CreateTime = m.CreateTime,
                             AccountStatus = m.AccountStatus,
                             memberStatusList = ms,
                             loginHistory = lastLoginTime,
                         }).AsEnumerable().DistinctBy(vm => vm.AccountId).Skip(skipAmount).Take(pageSize);

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
                            Name = m.Name,
                            Email = m.Email,
                            PhoneNo = m.PhoneNo
                        };

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = currentPage;
            return View(datas);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
