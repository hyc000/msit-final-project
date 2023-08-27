using prjCoreWebWantWant.Models;
using System.ComponentModel.DataAnnotations;

namespace prjCoreWebWantWant.ViewModels
{
    public class CResumeViewModel
    {
        [Display(Name = "履歷標題")]
        public string? ResumeTitle { get; set; }

        [Display(Name = "帳號")]
        public int AccountId { get; set; }

        public int ResumeId { get; set; }

        public int? TownId { get; set; }

        [Display(Name = "地址")]
        public string? Address { get; set; }

        [Display(Name = "自傳")]
        public string? Autobiography { get; set; }

        public int? WorkingHoursId { get; set; }

        [Display(Name = "照片")]
        public byte[]? Photo { get; set; }

        public int? OnTop { get; set; }

        [Display(Name = "任務類別")]
        public int? TaskNameId { get; set; }

        public bool IsExpertOrNot { get; set; }

        public int? CaseStatusId { get; set; }

        public virtual MemberAccount Account { get; set; } = null!;

        public virtual ICollection<ApplicationList> ApplicationLists { get; set; } = new List<ApplicationList>();

        public virtual CaseStatusList? CaseStatus { get; set; }

        public virtual ExpertResume? ExpertResume { get; set; }

        public virtual ICollection<MemberCollection> MemberCollections { get; set; } = new List<MemberCollection>();

        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        public virtual ICollection<ResumeCertificate> ResumeCertificates { get; set; } = new List<ResumeCertificate>();

        public virtual ICollection<ResumeKeywordList> ResumeKeywordLists { get; set; } = new List<ResumeKeywordList>();

        public virtual ICollection<ResumeSkill> ResumeSkills { get; set; } = new List<ResumeSkill>();

        public virtual TaskNameList? TaskName { get; set; }

        public virtual Town? Town { get; set; }

        public virtual WorkingTime? WorkingHours { get; set; }
    }
}
