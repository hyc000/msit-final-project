using Microsoft.AspNetCore.Mvc;
using prjCoreWebWantWant.Models;

namespace prjCoreWebWantWant.Controllers
{
    public class ApiForExpertController : Controller
    {
        //注入式
        private readonly NewIspanProjectContext _db;

        public ApiForExpertController(NewIspanProjectContext db)
        {
            _db = db;
        }


        //Skill Typt 的API

        public IActionResult SkillTyptAPI()
        {
         
            IEnumerable<string> data = _db.SkillTypes
                .Select(x=>x.SkillTypeName)
                .ToList();
            if(data != null)
            {
                return Json(data);
            }
            else
            {
                string notsearchdata = "沒有相關資料";
                return Json(notsearchdata);
            }
            
        }

        //Skill  的API
        public IActionResult SkillAPI(string skilltype)
        {
            int skilltypeID = _db.SkillTypes
                .Where(x => x.SkillTypeName == skilltype)
                .Select(x => x.SkillTypeId)
                .FirstOrDefault();

            IEnumerable<string> data = _db.Skills
                .Where(s=>s.SkillTypeId== skilltypeID)
               .Select(x => x.SkillName)
               .ToList();
            if (data != null)
            {
                return Json(data);
            }
            else
            {
                string notsearchdata = "沒有相關資料";
                return Json(notsearchdata);
            }
           
        }


        //Certificate Type的API

        public IActionResult CertificateTypeAPI()
        {

            IEnumerable<string> data = _db.CertificateTypes
                .Select(x => x.CertificateTypeName)
                .ToList();
            if (data != null)
            {
                return Json(data);
            }
            else
            {
                string notsearchdata = "沒有相關資料";
                return Json(notsearchdata);
            }

        }

        //Certificate 的API
        public IActionResult CertificateAPI(string certificatetype)
        {
            int certificatetypeID = _db.CertificateTypes
                .Where(x => x.CertificateTypeName == certificatetype)
                .Select(x => x.CertificateTypeId)
                .FirstOrDefault();

            IEnumerable<string> data = _db.Certificates
                .Where(s => s.CertificateTypeId == certificatetypeID)
               .Select(x => x.CertificateName)
               .ToList();
            if (data != null)
            {
                return Json(data);
            }
            else
            {
                string notsearchdata = "沒有相關資料";
                return Json(notsearchdata);
            }

        }
    }
}
