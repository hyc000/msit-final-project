using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjCoreWebWantWant.Models;

namespace WantTask.ViewModels
{
    //前後台要選擇多證照、多技能、照片的ViewModel
    public class CTaskDetailFrontandBackstage
    {
        public TaskList task { get; set; }
        public Resume resume { get; set; }
        public TaskPhoto photo { get; set; }

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

        //tasklist
        public int CaseId { get; set; }

        public int? AccountId { get; set; }

        public int? TaskNameId { get; set; }

        public string? TaskTitle { get; set; }

        public string? TaskDetail { get; set; }

        public int? WorkingHoursId { get; set; }

        public int? PayFrom { get; set; }

        public int? PayTo { get; set; }

        public int? PaymentId { get; set; }

        public int? PaymentDateId { get; set; }

        public int? SalaryId { get; set; }

        public int? TaskPlace { get; set; }

        public int? TownId { get; set; }

        public bool? WorkPlace { get; set; }

        public string? Address { get; set; }

        public string? RequiredNum { get; set; }

        public string? TaskPeriod { get; set; }

        public string? TaskStartHour { get; set; }

        public string? TaskEndHour { get; set; }

        public string? TaskStartDate { get; set; }

        public string? TaskEndDate { get; set; }

        public string? Requirement { get; set; }

        public string? HumanList { get; set; }

        public string? LanguageRequired { get; set; }

        public bool? ServiceStatus { get; set; }

        public int? StatusChangeReasonId { get; set; }

        public string? PublishOrNot { get; set; }

        public string? PublishStart { get; set; }

        public string? PublishEnd { get; set; }

        public int? CaseStatusId { get; set; }

        public DateTime? OnTop { get; set; }

        public string? DataCreateDate { get; set; }

        public string? DataModifyDate { get; set; }

        public string? DataModifyPerson { get; set; }

        public bool? IsExpert { get; set; }

        public virtual ICollection<ApplicationList> ApplicationLists { get; set; } = new List<ApplicationList>();

       // public virtual ICollection<CaseSkill> CaseSkills { get; set; } = new List<CaseSkill>();

        public virtual ICollection<ExpertApplication> ExpertApplications { get; set; } = new List<ExpertApplication>();

        public virtual ICollection<MemberCollection> MemberCollections { get; set; } = new List<MemberCollection>();

        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        public virtual Payment? Payment { get; set; }

        public virtual PaymentDate? PaymentDate { get; set; }

        public virtual Salary? Salary { get; set; }

      //  public virtual ICollection<TaskCertificate> TaskCertificates { get; set; } = new List<TaskCertificate>();

        public virtual ICollection<TaskKeywordList> TaskKeywordLists { get; set; } = new List<TaskKeywordList>();

        public virtual TaskNameList? TaskName { get; set; }

        public virtual ICollection<TaskPhoto> TaskPhotos { get; set; } = new List<TaskPhoto>();

    //    public virtual ICollection<TaskSkill> TaskSkills { get; set; } = new List<TaskSkill>();

        public virtual Town? Town { get; set; }

        //taskskill
        //public int TaskSkillId { get; set; }

        //public int CaseId { get; set; }

        //public int SkillId { get; set; }

        //public virtual TaskList Case { get; set; } = null!;

        //public virtual Skill Skill { get; set; } = null!;

        //skilltype
        //public int SkillTypeId { get; set; }

        //public string? SkillTypeName { get; set; }

        //public virtual ICollection<Skill> Skills { get; set; } = new List<Skill>();

        //skill
        //public int SkillId { get; set; }

        //public int SkillTypeId { get; set; }

        //public string? SkillName { get; set; }

       // public virtual ICollection<CaseSkill> CaseSkills { get; set; } = new List<CaseSkill>();

        //public virtual ICollection<ResumeSkill> ResumeSkills { get; set; } = new List<ResumeSkill>();

        //public virtual SkillType SkillType { get; set; } = null!;

        // public virtual ICollection<TaskSkill> TaskSkills { get; set; } = new List<TaskSkill>();

        //taskcer
        //public int TaskCertificateId { get; set; }

        //public int CaseId { get; set; }

        //public int CertficateId { get; set; }

        ////public virtual TaskList Case { get; set; } = null!;   不知道這是幹嘛的
         
        //public virtual Certificate Certficate { get; set; } = null!;

        //certype
       // public int CertificateTypeId { get; set; }

       //// public string? CertificateTypeName { get; set; }

       // public virtual ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();





        //cer
       // public int CertificateId { get; set; }

       // //public int CertificateTypeId { get; set; }

       // public string? CertificateName { get; set; }

       // public virtual CertificateType CertificateType { get; set; } = null!;

       // public virtual ICollection<ResumeCertificate> ResumeCertificates { get; set; } = new List<ResumeCertificate>();

       // // public virtual ICollection<TaskCertificate> TaskCertificates { get; set; } = new List<TaskCertificate>();

       // //taskphoto
       // public int TaskPhotoId { get; set; }

       //// public int CaseId { get; set; }

       // public byte[]? Photo { get; set; }

        // public virtual TaskList Case { get; set; } = null!;





        //MemberAccount
        public MemberAccount memberAccount { get; set; }





        //skill

        public TaskSkill taskSkill { get; set; }
        public SkillType skillType { get; set; }
        public Skill skill { get; set; }

        //cer
        public TaskCertificate taskCer { get; set; }
        public CertificateType certificateType { get; set; }
        public Certificate certificate { get; set; }


        //多Cer證照
        private List<CCertificateName> FindCertificate()
        {
            NewIspanProjectContext _context = new NewIspanProjectContext();
            List<CCertificateName> certificateName = _context.Certificates
                .Include(x => x.TaskCertificates.Where(x => x.CaseId == task.CaseId))
                .Select(x => new CCertificateName
                {
                    CertificateName = x.CertificateName,
                    CertificateTypeName = x.CertificateType.CertificateTypeName
                })
                .ToList();

            return certificateName;
        }
        public IEnumerable<string> certificatename
        {
            get
            {
                List<CCertificateName> name = FindCertificate();
                return name.Select(x => x.CertificateName);
            }
        }

        public IEnumerable<string> certificatetypename
        {
            get
            {
                List<CCertificateName> name = FindCertificate();
                return name.Select(x => x.CertificateTypeName);
            }
        }

        //多Skill
        private List<CSkillName> FindSkill()
        {
            NewIspanProjectContext _context = new NewIspanProjectContext();
            List<CSkillName> skillName = _context.Skills
                .Include(x => x.TaskSkills.Where(x => x.CaseId == task.CaseId))
                .Select(x => new CSkillName
                {
                    SkillName = x.SkillName,
                    SkillTypeName = x.SkillType.SkillTypeName
                })
                .ToList();

            return skillName;
        }
        public IEnumerable<string> skillname
        {
            get
            {
                List<CSkillName> name = FindSkill();
                return name.Select(x => x.SkillName);
            }
        }

        public IEnumerable<string> skilltypename
        {
            get
            {
                List<CSkillName> name = FindSkill();
                return name.Select(x => x.SkillTypeName);
            }
        }


    }
}
