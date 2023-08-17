
using prjCoreWebWantWant.Models;

namespace WantTask.ViewModels
{
    public class CTaskDetailViewModel
    {                        
        public int txtCaseId { get; set; }

        public int? txtAccountId { get; set; }

        public int? txtTaskNameId { get; set; }

        public string? txtTaskTitle { get; set; }

        public string? txtTaskDetail { get; set; }

        public int? txtWorkingHoursId { get; set; }

        public int? txtPayFrom { get; set; }

        public int? txtPayTo { get; set; }

        public int? txtSalaryId { get; set; }

        public int? txtTaskPlace { get; set; }

        public int? txtTownId { get; set; }

        public bool? txtWorkPlace { get; set; }

        public string? txtAddress { get; set; }

        public string? txtRequiredNum { get; set; }

        public string? txtTaskPeriod { get; set; }

        public string? txtTaskStart { get; set; }

        public string? txtTaskEnd { get; set; }

        public string? txtRequirement { get; set; }

        public string? txtHumanList { get; set; }

        public string? txtLanguageRequired { get; set; }

        public int? txtSkillRequiredId { get; set; }

        public int? txtCertificateRequiredId { get; set; }

        public bool? txtServiceStatus { get; set; }

        public int? txtStatusChangeReasonId { get; set; }

        public string? txtPublishOrNot { get; set; }

        public string? txtPublishStart { get; set; }

        public string? txtPublishEnd { get; set; }

        public int? txtCaseStatusId { get; set; }

        public int? txtOnTop { get; set; }

        public string? txtDataCreateDate { get; set; }

        public string? txtDataModifyDate { get; set; }

        public string? txtDataModifyPerson { get; set; }

        public bool? txtIsExpert { get; set; }

        public TaskList task { get; set; }

        public virtual ICollection<ApplicationList> ApplicationLists { get; set; } = new List<ApplicationList>();

        public virtual ICollection<CaseSkill> CaseSkills { get; set; } = new List<CaseSkill>();

        public virtual ICollection<ExpertApplication> ExpertApplications { get; set; } = new List<ExpertApplication>();

        public virtual ICollection<MemberCollection> MemberCollections { get; set; } = new List<MemberCollection>();

        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        public virtual ICollection<TaskKeywordList> TaskKeywordLists { get; set; } = new List<TaskKeywordList>();

        public virtual ICollection<TaskPhoto> TaskPhotos { get; set; } = new List<TaskPhoto>();
    }
}
