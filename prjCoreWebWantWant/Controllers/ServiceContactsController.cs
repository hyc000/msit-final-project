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

		public IActionResult List(CKeywordViewModel vm)
		{
			NewIspanProjectContext db = new NewIspanProjectContext();
			IEnumerable<ServiceContact> datas = null;
			if (string.IsNullOrEmpty(vm.txtKeyword))
				datas = from s in db.ServiceContacts
						select s;
			else
				datas = db.ServiceContacts.Where(s => s.Account.Name.ToUpper().Contains(vm.txtKeyword.ToUpper())
					|| s.Account.PhoneNo.ToUpper().Contains(vm.txtKeyword.ToUpper())
					|| s.Account.Email.ToUpper().Contains(vm.txtKeyword.ToUpper()));

			return View(datas);
		}

		public IActionResult Index()
        {           
            return View();
        }
               
    }
}
