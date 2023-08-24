using Microsoft.AspNetCore.Mvc;
using prjCoreWebWantWant.Models;
using prjCoreWebWantWant.ViewModels;
using prjCoreWantMember.ViewModels;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using X.PagedList;

namespace prjCoreWebWantWant.Controllers
{
    public class BackstageManagementController : Controller
    {
        private readonly NewIspanProjectContext _context;

        public BackstageManagementController(NewIspanProjectContext context)
        {
            _context = context;
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("List");

            NewIspanProjectContext db = new NewIspanProjectContext();            
            CBackstageManagementViewModel vm = GetViewModelById(id);

            if (vm == null)
                return RedirectToAction("List");

            return View(vm);
        }

        [HttpPost]
        public ActionResult Edit(int id, CBackstageManagementViewModel vm)
        {
            if (ModelState.IsValid)
            {
                NewIspanProjectContext db = new NewIspanProjectContext();              
                CBackstageManagementViewModel pDb = GetViewModelById(id);

                if (pDb != null)
                {                   
                    pDb.Name = vm.Name;
                    pDb.Email = vm.Email;
                    pDb.PhoneNo = vm.PhoneNo;
                    pDb.AccountStatus = vm.AccountStatus;
                    pDb.memberStatusList.StatusChangeReason = vm.memberStatusList.StatusChangeReason;
                                       
                    db.SaveChanges();
                }

                return RedirectToAction("List");
            }

            return View(vm);
        }

        private CBackstageManagementViewModel GetViewModelById(int? id)
        {
            NewIspanProjectContext db = new NewIspanProjectContext();
           
            CBackstageManagementViewModel vm = new CBackstageManagementViewModel
            {
                //memberAccount = db.MemberAccounts.FirstOrDefault(m => m.AccountId == id),
                memberStatusList = db.MemberStatusLists.FirstOrDefault(ms => ms.AccountId == id)
            };

            return vm;
        }

        //public IActionResult List(CKeywordViewModel vm, int? page)
        //{
        //    NewIspanProjectContext db = new NewIspanProjectContext();
        //    IEnumerable<CBackstageManagementViewModel> datas = null;

        //    int pageSize = 8;
        //    int pageNumber = page ?? 1;

        //    var pagedData = datas.Skip((pageNumber - 1) * pageSize).Take(pageSize);

        //    ViewBag.CurrentPage = pageNumber;
        //    ViewBag.TotalPages = (int)Math.Ceiling((double)datas.Count() / pageSize);

        //    return View(pagedData);
        //}

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
                             AccountId = m.AccountId,
                             Name = m.Name,
                             Email = m.Email,
                             PhoneNo = m.PhoneNo,
                             CreateTime = m.CreateTime,
                             AccountStatus = m.AccountStatus,
                             memberStatusList = ms,
                             loginHistory = lastLoginTime,
                         }).AsEnumerable().DistinctBy(vm => vm.AccountId).Take(8);

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

            return View(datas);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
