using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjCoreWantMember.ViewModels;
using prjCoreWebWantWant.Models;
using prjCoreWebWantWant.ViewModels;

namespace prjCoreWebWantWant.Controllers
{
    public class ServiceContactsController : Controller
    {
        private readonly NewIspanProjectContext _context;

        public ServiceContactsController(NewIspanProjectContext context)
        {
            _context = context;
        }

        public IActionResult SaveMessage()
        {
            NewIspanProjectContext db = new NewIspanProjectContext();
            IEnumerable<CServiceContactViewModel> datas = null;
 
            datas = from m in db.MemberAccounts
                    join s in db.ServiceContacts
                    on m.AccountId equals s.AccountId
                    select new CServiceContactViewModel
                    {
                        serviceContact = s,
                        AccountId = m.AccountId,
                        Name = m.Name,
                        Email = m.Email
                    };

            db.SaveChanges();
            return RedirectToAction("List");
        }

		public IActionResult List(CKeywordViewModel vm)
		{
			NewIspanProjectContext db = new NewIspanProjectContext();
			IEnumerable<CServiceContactViewModel> datas = null;
			if (string.IsNullOrEmpty(vm.txtKeyword))
                datas = from s in db.ServiceContacts
                        join m in db.MemberAccounts
                        on s.AccountId equals m.AccountId
                        select new CServiceContactViewModel
                        {
                            serviceContact=s,
                            AccountId = m.AccountId,
                            Name = m.Name,
                            Email = m.Email,
                            PhoneNo = m.PhoneNo
                        };
            else
				datas = from s in db.ServiceContacts
                        join m in db.MemberAccounts
                        on s.AccountId equals m.AccountId
                        where m.Name.Contains(vm.txtKeyword) || 
                        m.Email.Contains(vm.txtKeyword) || 
                        m.PhoneNo.Contains(vm.txtKeyword)
                        select new CServiceContactViewModel
                        {
                            serviceContact = s,
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
