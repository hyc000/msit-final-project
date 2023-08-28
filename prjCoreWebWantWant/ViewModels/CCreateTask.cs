using prjCoreWebWantWant.Models;
using System.ComponentModel.DataAnnotations;

namespace prjCoreWebWantWant.ViewModels
{
    public class CCreateTask
    {
        public int CaseId { get; set; }

        public int? AccountId { get; set; }

        public int? TaskNameId { get; set; }
        public string? PhotoPath { get; set; }
        public byte[]? Photo { get; set; }

        //任務內容
        [Display(Name ="任務標題")]
        public string? TaskTitle { get; set; }

        [Display(Name = "任務內容")]
        public string? TaskDetail { get; set; }

        [Display(Name = "需求人數")]
        public string? RequiredNum { get; set; }

        //任務時間
        [Display(Name = "開始日期")]
        public string? TaskStartDate { get; set; }

        [Display(Name = "結束日期")]
        public string? TaskEndDate { get; set; }
        [Display(Name = "開始時間，請輸入時段，例如：早班：08：00~16：00")]
        public string? TaskStartHour { get; set; }
        [Display(Name = "結束時間，請輸入時段，例如：中班：13：00~17：00，若無則免填")]
        public string? TaskEndHour { get; set; }

        //任務地點

        //要有select選城市
        public int? TownId { get; set; }

        public bool? WorkPlace { get; set; }

        [Display(Name = "請輸入地址")]
        public string? Address { get; set; }


        //任務報酬
        [Display(Name = "最低金額")]
        public int? PayFrom { get; set; }

        [Display(Name = "最高金額")]
        public int? PayTo { get; set; }

        [Display(Name = "支薪方式")]
        public int? PaymentId { get; set; }

        [Display(Name = "支薪日")]
        public int? PaymentDateId { get; set; }

      
        
        
        public int? WorkingHoursId { get; set; }

      

        public int? SalaryId { get; set; }

        public int? TaskPlace { get; set; }

       

      

        public string? TaskPeriod { get; set; }

   

   

        public string? Requirement { get; set; }

        public string? HumanList { get; set; }

        public string? LanguageRequired { get; set; }

        public bool? ServiceStatus { get; set; }

        public int? StatusChangeReasonId { get; set; }

        public string? PublishOrNot { get; set; }

        public string? PublishStart { get; set; }

        public string? PublishEnd { get; set; }

        public int? CaseStatusId { get; set; }

        public int? OnTop { get; set; }

        public string? DataCreateDate { get; set; }

        public string? DataModifyDate { get; set; }

        public string? DataModifyPerson { get; set; }

        public bool? IsExpert { get; set; }

        public virtual ICollection<ApplicationList> ApplicationLists { get; set; } = new List<ApplicationList>();

        public virtual ICollection<CaseSkill> CaseSkills { get; set; } = new List<CaseSkill>();

        public virtual ICollection<ExpertApplication> ExpertApplications { get; set; } = new List<ExpertApplication>();

        public virtual ICollection<MemberCollection> MemberCollections { get; set; } = new List<MemberCollection>();

        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        public virtual Payment? Payment { get; set; }

        public virtual PaymentDate? PaymentDate { get; set; }

        public virtual Salary? Salary { get; set; }

        public virtual ICollection<TaskCertificate> TaskCertificates { get; set; } = new List<TaskCertificate>();

        public virtual ICollection<TaskKeywordList> TaskKeywordLists { get; set; } = new List<TaskKeywordList>();

        public virtual TaskNameList? TaskName { get; set; }

        public virtual ICollection<TaskPhoto> TaskPhotos { get; set; } = new List<TaskPhoto>();

        public virtual ICollection<TaskSkill> TaskSkills { get; set; } = new List<TaskSkill>();

        public virtual Town? Town { get; set; }



    }
}
