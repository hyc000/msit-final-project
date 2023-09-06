using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using prjCoreWantMember.ViewModels;
using prjCoreWebWantWant.Models;
using prjCoreWebWantWant.ViewModels;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;

namespace prjWantWant_yh_CoreMVC.Controllers
{
    public class BackstageController : Controller
    {
        private readonly NewIspanProjectContext _context;

        private readonly IWebHostEnvironment _host;
        public BackstageController(NewIspanProjectContext context, IWebHostEnvironment host)
        {
            _context = context;
            _host = host;
        }

        public int GetAccountID()
        {
            //if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            //{

            //}
            string userDataJson = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
            CLoginUser loggedInUser = JsonSerializer.Deserialize<CLoginUser>(userDataJson); 
            int id = loggedInUser.AccountId; //抓登入者的id                                                                             
            return id;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Resume p,int selectedTownId, int selectedSkillId1,int selectedSkillId2,int selectedSkillId3, IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                string filePath = Path.Combine(_host.WebRootPath, "Backstage", "img", imageFile.FileName);
                //var imagePath = "~/Backstage/img/" + Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                string imagePath = "/Backstage/img/" + imageFile.FileName;

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.CopyTo(fileStream);
                }

                p.PhotoPath = imagePath;
            }
                p.CaseStatusId = 23;
                p.TownId = selectedTownId;
                //p.Photo = new byte[] {selectedPhoto};
                p.DataModifyDate = DateTime.Now.Date.ToString("yyyy-MM-dd");
                _context.Resumes.Add(p);
                _context.SaveChanges();

                 ResumeSkill resumeSkill1 = new ResumeSkill()
                {
                    ResumeId = p.ResumeId,
                    SkillId = selectedSkillId1
                };
                _context.Add(resumeSkill1);
                _context.SaveChanges();

                ResumeSkill resumeSkill2 = new ResumeSkill()
                {
                    ResumeId = p.ResumeId,
                    SkillId = selectedSkillId2
                };
                _context.Add(resumeSkill2);
                _context.SaveChanges();

                ResumeSkill resumeSkill3 = new ResumeSkill()
                {
                    ResumeId = p.ResumeId,
                    SkillId = selectedSkillId3
                };
                _context.Add(resumeSkill3);
                _context.SaveChanges();
                return RedirectToAction("ResumeList");
        }

        public IActionResult ResumeUneditable(int? id)
        {
            var q = _context.Resumes
                    .Include(t => t.Town.City)
                    .Where(r => r.IsExpertOrNot == false && r.ResumeId == id && r.CaseStatusId != 22).FirstOrDefault();
            return View(q);
        }
        public IActionResult ResumeUneditableforRecycleBin(int? id)
        {
            var q = _context.Resumes
                    .Include(t => t.Town.City)
                    .Where(r => r.IsExpertOrNot == false && r.ResumeId == id && r.CaseStatusId == 22).FirstOrDefault();
            return View(q);
        }

        public IActionResult ResumeRecover(int? id)
        {
            if (id != null)
            {
                Resume resume = _context.Resumes.FirstOrDefault(p => p.ResumeId == id);
                if (resume != null)
                {
                    resume.CaseStatusId = 23;
                    //_context.Resumes.Remove(resume);
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("RecycleBin");
        }

        public IActionResult ResumeList()   //todo 將技能 證照直接顯示在外面
        {
            var q = _context.Resumes
                    .Include(t => t.Town.City)                                           
                    .Where(r => r.IsExpertOrNot == false && r.AccountId == GetAccountID() && r.CaseStatusId == 23);
            return View(q);
        }
        public IActionResult ResumeDelete(int? id)
        {
            if (id != null)
            {
                Resume resume = _context.Resumes.FirstOrDefault(p => p.ResumeId == id);
                if (resume != null)
                {
                    resume.CaseStatusId = 22;
                    resume.RemoveDate = DateTime.Now.Date.ToString("yyyy-MM-dd");
                    //_context.Resumes.Remove(resume);
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("ResumeList");
        }

        public IActionResult DeleteAll()
        {
            var matchingResumes = _context.Resumes
                                  .Where(r => r.IsExpertOrNot == false && r.CaseStatusId == 22 && r.AccountId == GetAccountID());

            foreach (var resume in matchingResumes)
            {
                resume.CaseStatusId = 24;
            }

            // 儲存變更到資料庫
            _context.SaveChanges();
            
            return RedirectToAction("RecycleBin");
        }

        public IActionResult ResumeDeleteforRecycleBin(int? id)
        {
            if (id != null)
            {
                Resume resume = _context.Resumes.FirstOrDefault(p => p.ResumeId == id);
                if (resume != null)
                {
                    resume.CaseStatusId = 24;
                    //_context.Resumes.Remove(resume);
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("RecycleBin");
        }

        //public IActionResult ResumeEdit()
        //{
        //    return View();
        //}

        public IActionResult ResumeEdit(int? id)
        {
            if (id == null)
                return RedirectToAction("ResumeList");
            //Resume resume = _context.Resumes.FirstOrDefault(p => p.ResumeId == id);

            Resume resume = _context.Resumes
                            .Include(t => t.Town.City)
                            .Where(r => r.IsExpertOrNot == false && r.ResumeId == id && r.CaseStatusId == 23)
                            .FirstOrDefault(p => p.ResumeId == id);
            if (resume == null)
                return RedirectToAction("ResumeList");
            return View(resume);
        }

        [HttpPost]
        public IActionResult ResumeEdit(Resume pIn,int selectedTownId, IFormFile imageFile)
        {
            Resume resume = _context.Resumes.FirstOrDefault(p => p.ResumeId == pIn.ResumeId);
            if (resume != null)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    string filePath = Path.Combine(_host.WebRootPath, "Backstage", "img", imageFile.FileName);
                    //var imagePath = "~/Backstage/img/" + Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                    string imagePath = "/Backstage/img/" + imageFile.FileName;

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        imageFile.CopyTo(fileStream);
                    }

                    resume.PhotoPath = imagePath;
                }
                resume.TownId = selectedTownId;
                resume.Autobiography = pIn.Autobiography;
                resume.Address = pIn.Address;
                resume.DataModifyDate = DateTime.Now.Date.ToString("yyyy-MM-dd");
                _context.SaveChanges();
            }
            return RedirectToAction("ResumeList");
        }

        public IActionResult TaskCollection()
        {
            //var q = from mc in _context.MemberCollections
            //        join tl in _context.TaskLists on mc.CaseId equals tl.CaseId
            //        join tc in _context.TaskCertificates on tl.CaseId equals tc.CaseId
            //        where mc.AccountId == GetAccountID() && mc.CaseId != null
            //        select new CMemberCollectionViewModel
            //        {
            //            TaskTitle = tl.TaskTitle,
            //            TaskDetail = tl.TaskDetail,
            //            RequiredNum = tl.RequiredNum,
            //            PayFrom = tl.PayFrom,
            //            TaskNameId =tl.TaskNameId,
            //            PaymentId = tl.PaymentId,
            //            CaseId = mc.CaseId,
            //            CollectionDate = mc.CollectionDate,
            //        };

            var q = from mc in _context.MemberCollections
                    join tl in _context.TaskLists on mc.CaseId equals tl.CaseId
                    join tc in _context.TaskCertificates on tl.CaseId equals tc.CaseId
                    join ts in _context.TaskSkills on tl.CaseId equals ts.CaseId // 添加這個 join
                    where mc.AccountId == GetAccountID() && mc.CaseId != null           //要同時有task技能和task證照才會顯示出來
                    select new
                    {
                        Task = tl,
                        Certificate = tc.Certficate.CertificateName,
                        Member = mc,
                        Skill = ts.Skill.SkillName,
                    }
                    into combined
                    group combined by new
                    {
                        combined.Task.CaseId,
                        combined.Task.TaskTitle,
                        combined.Task.TaskDetail,
                        combined.Task.RequiredNum,
                        combined.Task.PayFrom,
                        combined.Task.TaskNameId,
                        combined.Task.PaymentId,
                        combined.Member.CollectionDate,
                         // 如果一個工作可以有多個技能，請包括這個以確保每個工作都在結果中
                    } into grouped
                    select new CMemberCollectionViewModel
                    {
                        TaskTitle = grouped.Key.TaskTitle,
                        TaskDetail = grouped.Key.TaskDetail,
                        RequiredNum = grouped.Key.RequiredNum,
                        PayFrom = grouped.Key.PayFrom,
                        TaskNameId = grouped.Key.TaskNameId,
                        PaymentId = grouped.Key.PaymentId,
                        CaseId = grouped.Key.CaseId,
                        CollectionDate = grouped.Key.CollectionDate,
                        Certificate = grouped.Select(g => g.Certificate).Distinct().ToList(),
                        Skill = grouped.Select(g => g.Skill).Distinct().ToList(),
                    };


            return View(q.ToList());
        }

        public IActionResult PartialCollection(string category, CKeywordViewModel vm)
        {
            var q = from mc in _context.MemberCollections
                    join tl in _context.TaskLists on mc.CaseId equals tl.CaseId
                    join tc in _context.TaskCertificates on tl.CaseId equals tc.CaseId
                    join ts in _context.TaskSkills on tl.CaseId equals ts.CaseId // 添加這個 join
                    where mc.AccountId == GetAccountID() && mc.CaseId != null && (category == null ? true : category == tl.TaskName.TaskName)
                                                                                 //三元運算式
                    select new
                    {
                        Task = tl,
                        Certificate = tc.Certficate.CertificateName,
                        Member = mc,
                        Skill = ts.Skill.SkillName,
                    }
                    into combined
                    group combined by new
                    {
                        combined.Task.CaseId,
                        combined.Task.TaskTitle,
                        combined.Task.TaskDetail,
                        combined.Task.RequiredNum,
                        combined.Task.PayFrom,
                        combined.Task.TaskNameId,
                        combined.Task.PaymentId,
                        combined.Member.CollectionDate,
                        // 如果一個工作可以有多個技能，請包括這個以確保每個工作都在結果中
                    } into grouped
                    select new CMemberCollectionViewModel
                    {
                        TaskTitle = grouped.Key.TaskTitle,
                        TaskDetail = grouped.Key.TaskDetail,
                        RequiredNum = grouped.Key.RequiredNum,
                        PayFrom = grouped.Key.PayFrom,
                        TaskNameId = grouped.Key.TaskNameId,
                        PaymentId = grouped.Key.PaymentId,
                        CaseId = grouped.Key.CaseId,
                        CollectionDate = grouped.Key.CollectionDate,
                        Certificate = grouped.Select(g => g.Certificate).Distinct().ToList(),
                        Skill = grouped.Select(g => g.Skill).Distinct().ToList(),
                    };

            //var q = from mc in _context.MemberCollections
            //        join tl in _context.TaskLists on mc.CaseId equals tl.CaseId
            //        where mc.AccountId == GetAccountID() && mc.CaseId != null && category == tl.TaskName.TaskName
            //        select new CMemberCollectionViewModel
            //        {
            //            TaskTitle = tl.TaskTitle,
            //            TaskDetail = tl.TaskDetail,
            //            RequiredNum = tl.RequiredNum,
            //            PayFrom = tl.PayFrom,
            //            TaskNameId = tl.TaskNameId,
            //            PaymentId = tl.PaymentId,
            //            CaseId = mc.CaseId,
            //            CollectionDate = mc.CollectionDate
            //        };
            //if (category != null)
            //{
            //    q.Where(mc => mc.TaskName.TaskName == category);
            //}
            return PartialView(q.ToList());
        }
        public IActionResult AddCollection(MemberCollection mc , int? id)
        {
            mc.CaseId = id;
            mc.AccountId = GetAccountID();
            mc.CollectionDate = DateTime.Now.Date.ToString("yyyy-MM-dd");
            _context.Add(mc);
            _context.SaveChanges();
            return RedirectToAction("ListNew", "Task");
            //return new EmptyResult();
        }

        public IActionResult DeleteCollection(int? id)
        {
            if (id != null)
            {
                MemberCollection memberCollection = _context.MemberCollections.FirstOrDefault(p => p.CaseId == id && p.AccountId == GetAccountID());
                if (memberCollection != null)
                {
                    _context.MemberCollections.Remove(memberCollection);
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("TaskCollection");
        }

        public IActionResult Apply(int? resumeId,int? caseId)
        {
            ApplicationList applicationList = new ApplicationList()
            {
                ResumeId = resumeId,        //todo 還沒抓到履歷id(多履歷的問題)
                CaseId = caseId,
                CaseStatusId = 21,
                ApplicationDate = DateTime.Now.Date.ToString("yyyy-MM-dd"),
                
            };
            _context.Add(applicationList);
            _context.SaveChanges(true);
            return RedirectToAction("TaskCollection");
        }

        public IActionResult AcceptPartial(string category)
        {
            var q = from al in _context.ApplicationLists
                    join tl in _context.TaskLists on al.CaseId equals tl.CaseId
                    join ts in _context.TaskSkills on al.CaseId equals ts.CaseId
                    join tc in _context.TaskCertificates on al.CaseId equals tc.CaseId
                    join tn in _context.TaskNameLists on tl.TaskNameId equals tn.TaskNameId
                    where al.Resume.AccountId == GetAccountID() && al.Resume.ResumeId == al.ResumeId && al.CaseStatusId == 1 && (category == null ? true : category == tl.TaskName.TaskName)
                    select new
                    {
                        AppList = al,
                        Task = tl,
                        Skill = ts.Skill.SkillName,
                        Certificate = tc.Certficate.CertificateName,
                    }
                    into combined
                    group combined by new
                    {
                        combined.Task.CaseId,
                        combined.Task.TaskTitle,
                        combined.Task.TaskDetail,
                        combined.Task.RequiredNum,
                        combined.Task.PayFrom,
                        combined.Task.TaskNameId,
                        combined.Task.PaymentId,
                        combined.AppList.ApplicationDate,
                        // 如果一個工作可以有多個技能，請包括這個以確保每個工作都在結果中
                    } into grouped
                    select new CMemberCollectionViewModel
                    {
                        TaskTitle = grouped.Key.TaskTitle,
                        TaskDetail = grouped.Key.TaskDetail,
                        RequiredNum = grouped.Key.RequiredNum,
                        PayFrom = grouped.Key.PayFrom,
                        TaskNameId = grouped.Key.TaskNameId,
                        PaymentId = grouped.Key.PaymentId,
                        CaseId = grouped.Key.CaseId,
                        ApplicationDate = grouped.Key.ApplicationDate,
                        Skill = grouped.Select(g => g.Skill).Distinct().ToList(),
                        Certificate = grouped.Select(g => g.Certificate).Distinct().ToList()
                    };
            return PartialView(q.ToList());            
        }

        public IActionResult RefusePartial(string category)
        {
            var q = from al in _context.ApplicationLists
                    join tl in _context.TaskLists on al.CaseId equals tl.CaseId
                    join ts in _context.TaskSkills on al.CaseId equals ts.CaseId
                    join tc in _context.TaskCertificates on tl.CaseId equals tc.CaseId
                    join tn in _context.TaskNameLists on tl.TaskNameId equals tn.TaskNameId
                    where al.Resume.AccountId == GetAccountID() && al.Resume.ResumeId == al.ResumeId && al.CaseStatusId == 2  && (category == null ? true : category == tl.TaskName.TaskName)
                    select new
                    {
                        AppList = al,
                        Task = tl,
                        Skill = ts.Skill.SkillName,
                        Certificate = tc.Certficate.CertificateName,
                    }
                into combined
                    group combined by new
                    {
                        combined.Task.CaseId,
                        combined.Task.TaskTitle,
                        combined.Task.TaskDetail,
                        combined.Task.RequiredNum,
                        combined.Task.PayFrom,
                        combined.Task.TaskNameId,
                        combined.Task.PaymentId,
                        combined.AppList.ApplicationDate,
                        // 如果一個工作可以有多個技能，請包括這個以確保每個工作都在結果中
                    } into grouped
                    select new CMemberCollectionViewModel
                    {
                        TaskTitle = grouped.Key.TaskTitle,
                        TaskDetail = grouped.Key.TaskDetail,
                        RequiredNum = grouped.Key.RequiredNum,
                        PayFrom = grouped.Key.PayFrom,
                        TaskNameId = grouped.Key.TaskNameId,
                        PaymentId = grouped.Key.PaymentId,
                        CaseId = grouped.Key.CaseId,
                        ApplicationDate = grouped.Key.ApplicationDate,
                        Skill = grouped.Select(g => g.Skill).Distinct().ToList(),
                        Certificate = grouped.Select(g => g.Certificate).Distinct().ToList(),
                    };
            return PartialView(q.ToList());
        }

        public IActionResult ApplicationRecord()   //todo 沒有正確顯示應徵紀錄 還沒抓到履歷id(多履歷的問題)
        {
            //var q = from al in _context.ApplicationLists
            //        join tl in _context.TaskLists on al.CaseId equals tl.CaseId 
            //        join ts in _context.TaskSkills on al.CaseId equals ts.CaseId
            //        where al.Resume.AccountId == GetAccountID() && al.Resume.ResumeId == al.ResumeId /*&& al.CaseStatusId == 21*/
            //        select new CMemberCollectionViewModel                                            //條件錯誤 21是已投遞未處理 但此處要全部的資料
            //        {
            //            TaskTitle = tl.TaskTitle,
            //            TaskDetail = tl.TaskDetail,
            //            RequiredNum = tl.RequiredNum,
            //            PayFrom = tl.PayFrom,
            //            TaskNameId = tl.TaskNameId,
            //            PaymentId = tl.PaymentId,
            //            CaseId = al.CaseId,
            //            ApplicationDate = al.ApplicationDate,
            //            Skill = ts.Skill.SkillName
            //        };

            //var q = from al in _context.ApplicationLists
            //        join tl in _context.TaskLists on al.CaseId equals tl.CaseId 
            //        join ts in _context.TaskSkills on al.CaseId equals ts.CaseId
            //        join tc in _context.TaskCertificates on tl.CaseId equals tc.CaseId
            //        where al.Resume.AccountId == GetAccountID() && al.Resume.ResumeId == al.ResumeId  //列出全部應徵紀錄

            //        select new
            //        {
            //            AppList = al,
            //            Task = tl,
            //            Skill = ts.Skill.SkillName,
            //            Certificate = tc.Certficate.CertificateName,
            //        }
            //        into combined
            //        group combined by new
            //        {
            //            combined.Task.CaseId,
            //            combined.Task.TaskTitle,
            //            combined.Task.TaskDetail,
            //            combined.Task.RequiredNum,
            //            combined.Task.PayFrom,
            //            combined.Task.TaskNameId,
            //            combined.Task.PaymentId,
            //            combined.AppList.ApplicationDate,
            //             // 如果一個工作可以有多個技能，請包括這個以確保每個工作都在結果中
            //        } into grouped
            //        select new CMemberCollectionViewModel
            //        {
            //            TaskTitle = grouped.Key.TaskTitle,
            //            TaskDetail = grouped.Key.TaskDetail,
            //            RequiredNum = grouped.Key.RequiredNum,
            //            PayFrom = grouped.Key.PayFrom,
            //            TaskNameId = grouped.Key.TaskNameId,
            //            PaymentId = grouped.Key.PaymentId,
            //            CaseId = grouped.Key.CaseId,
            //            ApplicationDate = grouped.Key.ApplicationDate,
            //            Skill = grouped.Select(g => g.Skill).Distinct().ToList(),
            //            Certificate = grouped.Select(g => g.Certificate).Distinct().ToList(),
            //        };

            return View();
            //return View(q.ToList());
        }

        public IActionResult PartialApplicationRecord(string category)   //todo 沒有正確顯示應徵紀錄 還沒抓到履歷id(多履歷的問題)
        {
            //var q = from al in _context.ApplicationLists
            //        join tl in _context.TaskLists on al.CaseId equals tl.CaseId
            //        join tn in _context.TaskNameLists on tl.TaskNameId equals tn.TaskNameId
            //        where al.Resume.AccountId == GetAccountID() && al.Resume.ResumeId == al.ResumeId && tn.TaskName == category /*&& al.CaseStatusId == 21*/
            //        select new CMemberCollectionViewModel                                            //條件錯誤 21是已投遞未處理 但此處要全部的資料
            //        {
            //            TaskTitle = tl.TaskTitle,
            //            TaskDetail = tl.TaskDetail,
            //            RequiredNum = tl.RequiredNum,
            //            PayFrom = tl.PayFrom,
            //            TaskNameId = tl.TaskNameId,
            //            PaymentId = tl.PaymentId,
            //            CaseId = al.CaseId,
            //            ApplicationDate = al.ApplicationDate
            //        };
            var q = from al in _context.ApplicationLists
                    join tl in _context.TaskLists on al.CaseId equals tl.CaseId
                    join ts in _context.TaskSkills on al.CaseId equals ts.CaseId
                    join tn in _context.TaskNameLists on tl.TaskNameId equals tn.TaskNameId
                    join tc in _context.TaskCertificates on tl.CaseId equals tc.CaseId

                    where al.Resume.AccountId == GetAccountID() && al.Resume.ResumeId == al.ResumeId && (category == null ? true : category == tl.TaskName.TaskName)
                    select new
                    {
                        AppList = al,
                        Task = tl,
                        Skill = ts.Skill.SkillName,
                        Certificate = tc.Certficate.CertificateName,
                    }
                   into combined
                    group combined by new
                    {
                        combined.Task.CaseId,
                        combined.Task.TaskTitle,
                        combined.Task.TaskDetail,
                        combined.Task.RequiredNum,
                        combined.Task.PayFrom,
                        combined.Task.TaskNameId,
                        combined.Task.PaymentId,
                        combined.AppList.ApplicationDate,
                        // 如果一個工作可以有多個技能，請包括這個以確保每個工作都在結果中
                    } into grouped
                    select new CMemberCollectionViewModel
                    {
                        TaskTitle = grouped.Key.TaskTitle,
                        TaskDetail = grouped.Key.TaskDetail,
                        RequiredNum = grouped.Key.RequiredNum,
                        PayFrom = grouped.Key.PayFrom,
                        TaskNameId = grouped.Key.TaskNameId,
                        PaymentId = grouped.Key.PaymentId,
                        CaseId = grouped.Key.CaseId,
                        ApplicationDate = grouped.Key.ApplicationDate,
                        Skill = grouped.Select(g => g.Skill).Distinct().ToList(),
                        Certificate = grouped.Select(g => g.Certificate).Distinct().ToList(),
                    };

            return PartialView(q.ToList());
        }

        public IActionResult GetTownId(string City ,string District)
        {
            var townId = _context.Cities
                         .Where(a => a.City1 == City)
                         .SelectMany(c => c.Towns)
                         .Where(c => c.Town1 == District)
                         .Select(c => c.TownId);

            return Json(townId);
        }

        //找出SkillId
        public IActionResult GetSkillId(string skillType ,string skillName)
        {
            var skillId = _context.SkillTypes
                         .Where(a => a.SkillTypeName == skillType)
                         .SelectMany(c => c.Skills)
                         .Where(c => c.SkillName == skillName)
                         .Select(c => c.SkillId);

            return Json(skillId);
        }

        public IActionResult GetCerId(string cerType, string cerName)
        {
            var cerId = _context.CertificateTypes
                         .Where(a => a.CertificateTypeName == cerType)
                         .SelectMany(c => c.Certificates)
                         .Where(c => c.CertificateName == cerName)
                         .Select(c => c.CertificateId);

            return Json(cerId);
        }

        public IActionResult PowerBI()
        {
            return View();
        }

        public IActionResult JobDetail(int? id)
        {
            var q = _context.TaskLists
                    .Include(c => c.Town.City).Where(t => t.CaseId == id).FirstOrDefault();
            return View(q);
        }

        public IActionResult RecycleBin()
        {
            var q = _context.Resumes
                    .Include(t => t.Town.City)
                    .Where(r => r.IsExpertOrNot == false && r.AccountId == GetAccountID() && r.CaseStatusId == 22);
            return View(q);
        }
    }
}
