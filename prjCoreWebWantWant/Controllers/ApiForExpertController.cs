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
                .Where(x=>x.SkillTypeName == skilltype)
                .Select(x=>x.SkillTypeId)
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

    }
}
