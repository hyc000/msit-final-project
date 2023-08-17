using Microsoft.AspNetCore.Mvc;
using prjCoreWebWantWant.Models;
using prjCoreWantMember.ViewModels;

namespace prjCoreWantMember.Controllers
{
    public class ExpertController : Controller
    {
        public IActionResult ExpertMainPage(CKeywordViewModel vm)
        {
            NewIspanProjectContext db = new NewIspanProjectContext();

            IEnumerable<CExpertInfoViewModel> datas = null;
            if (string.IsNullOrEmpty(vm.txtKeyword))
            {
                datas = from r in db.Resumes
                        join m in db.MemberAccounts
                        on r.AccountId equals m.AccountId
                        join er in db.ExpertResumes
                       on r.ResumeId equals er.ResumeId
                        where r.IsExpertOrNot == true
                        select new CExpertInfoViewModel { resume = r, memberAccount = m, expertResume = er };
            }
            else
            {
                datas = from r in db.Resumes
                        join m in db.MemberAccounts
                       on r.AccountId equals m.AccountId
                        join er in db.ExpertResumes
                        on r.ResumeId equals er.ResumeId
                        where r.IsExpertOrNot == true && (m.Name.ToUpper().Contains(vm.txtKeyword.ToUpper()) ||
                        r.Address.ToUpper().Contains(vm.txtKeyword.ToUpper()))
                        select new CExpertInfoViewModel { resume = r, memberAccount = m, expertResume = er };
            }
            return View(datas);
        }
        public IActionResult ExpertMemberPage(int? idforreal)
        {
            NewIspanProjectContext db = new NewIspanProjectContext();

            IEnumerable<CExpertInfoViewModel> datas = null;
            idforreal = 33;

            datas = from r in db.Resumes
                    join m in db.MemberAccounts
                    on r.AccountId equals m.AccountId
                    join er in db.ExpertResumes
                   on r.ResumeId equals er.ResumeId
                    where r.IsExpertOrNot == true && r.AccountId == idforreal
                    select new CExpertInfoViewModel { resume = r, memberAccount = m, expertResume = er };
            return View(datas);
        }

        public IActionResult EditExpertResume(int? idforResume)
        {
            NewIspanProjectContext db = new NewIspanProjectContext();

            IEnumerable<CExpertInfoViewModel> datas = null;
            idforResume = 22;

            datas = from r in db.Resumes
                    join m in db.MemberAccounts
                    on r.AccountId equals m.AccountId
                    join er in db.ExpertResumes
                   on r.ResumeId equals er.ResumeId
                    where r.IsExpertOrNot == true && r.ResumeId == idforResume
                    select new CExpertInfoViewModel { resume = r, memberAccount = m, expertResume = er };
            return View(datas);
        }
        public IActionResult AddExpertResume(int? idforAccount)
        {
            NewIspanProjectContext db = new NewIspanProjectContext();
            idforAccount = 33;
            CExpertInfoViewModel data = null;
            //data.resume.AccountId = (int)idforAccount;
            data = (from m in db.MemberAccounts
                    where m.AccountId == idforAccount
                    select new CExpertInfoViewModel { memberAccount = m }).FirstOrDefault();
            return View(data);
        }
    }

    
}
