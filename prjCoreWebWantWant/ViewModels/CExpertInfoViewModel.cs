
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjCoreWebWantWant.Models;

namespace prjCoreWantMember.ViewModels
{
    public class CExpertInfoViewModel
    {

        public ExpertResume expertResume { get; set; }

        public ExpertWork expertwork { get; set; }
        public ExpertWorkList expertWorkList { get; set; }


        //Resume
        public Resume resume { get; set; }

        public FileResult photo { get; set; }


        public string townName
        {
            get
            {
                NewIspanProjectContext db = new NewIspanProjectContext();
                string name = db.Towns.Where(x => x.TownId == this.resume.TownId).Select(x => x.Town1).FirstOrDefault();
                return name;
            }
        }
        private string FindCity()
        {
            NewIspanProjectContext db = new NewIspanProjectContext();
            var cityNameList = db.Towns
           .Where(x => x.TownId == this.resume.TownId)
           .Select(x => x.City.City1)
           .FirstOrDefault();

            return cityNameList;
        }


        public string cityName
        {
            get
            {
                return this.FindCity();

            }
        }


        //MemberAccount
        public MemberAccount memberAccount { get; set; }


        //SKILL  要可以複選
        public SkillType skillType { get; set; }
        public ResumeSkill resumeskill { get; set; }
        public Skill skill { get; set; }
        //證照 要可以複選
        public CertificateType certificatetype { get; set; }
        public Certificate certificate { get; set; }
        
        public ResumeCertificate resumecertificate { get; set; }
        private List <CCertificateName> FindCertificate()
        {
            //有多數的話
            NewIspanProjectContext db = new NewIspanProjectContext();
            List<CCertificateName> certificateName = db.Certificates
                .Include(x => x.ResumeCertificates.Where(x => x.ResumeId == resume.ResumeId))
                .Select(x=>new CCertificateName { CertificateName= x.CertificateName, 
                    CertificateTypeName= x.CertificateType.CertificateTypeName})
                .ToList();
             
            return certificateName;
        }
        public IEnumerable<string> certificatename { get
            {
                List<CCertificateName> name = FindCertificate();
                return name.Select(x=>x.CertificateName); }  }

        public IEnumerable<string> certificatetypename
        {
            get
            {
                List<CCertificateName> name = FindCertificate();
                return name.Select(x => x.CertificateTypeName);
            }
        }


        //評價
        public Rating rating { get; set; }
    }
}
