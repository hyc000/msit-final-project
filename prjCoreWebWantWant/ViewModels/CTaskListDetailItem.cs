using prjCoreWebWantWant.Models;

namespace WantTask.ViewModels
{
    public class CTaskListDetailItem
    {
        private TaskList _task = null;

        public TaskList task
        {
            get { return _task; }
            set { _task = value; }
        }

        public CTaskListDetailItem()
        {
            _task = new TaskList();
        }

        public int FId
        {
            get { return _task.CaseId; }
            set { _task.CaseId = value; }
        }      

            public int? AccountId { get; set; }

            public int? TaskNameId { get; set; }

        public string? FTitle
        {
            get { return _task.TaskTitle; }
            set { _task.TaskTitle = value; }
        }
        public string? FDetail
        {
            get { return _task.TaskDetail; }
            set { _task.TaskDetail = value; }
        }
        public int? FPayFrom
        {
            get { return _task.PayFrom; }
            set { _task.PayFrom = value; }
        }
        public int? FPayTo
        {
            get { return _task.PayTo; }
            set { _task.PayTo = value; }
        }


            public int? WorkingHoursId { get; set; }

        

            public int? SalaryId { get; set; }

            public int? TaskPlace { get; set; }

            public int? TownId { get; set; }

            public bool? WorkPlace { get; set; }

            public string? Address { get; set; }

            public string? RequiredNum { get; set; }

            public string? TaskPeriod { get; set; }

            public string? TaskStart { get; set; }

            public string? TaskEnd { get; set; }

            public string? Requirement { get; set; }

            public string? HumanList { get; set; }

            public string? LanguageRequired { get; set; }

            public int? SkillRequiredId { get; set; }

            public int? CertificateRequiredId { get; set; }

            public bool? ServiceStatus { get; set; }

            public int? StatusChangeReasonId { get; set; }

            public string? FPublishOrNot
        {
            get { return _task.PublishOrNot; }
            set { _task.PublishOrNot = value; }
        }


            public string? PublishStart { get; set; }

            public string? PublishEnd { get; set; }

            public int? CaseStatusId { get; set; }

            public int? OnTop { get; set; }

            public string? DataCreateDate { get; set; }

            public string? DataModifyDate { get; set; }

            public string? DataModifyPerson { get; set; }

            public bool? IsExpert { get; set; }

            //public TaskList task { get; set; }

            public virtual ICollection<ApplicationList> ApplicationLists { get; set; } = new List<ApplicationList>();

            public virtual ICollection<CaseSkill> CaseSkills { get; set; } = new List<CaseSkill>();

            public virtual ICollection<ExpertApplication> ExpertApplications { get; set; } = new List<ExpertApplication>();

            public virtual ICollection<MemberCollection> MemberCollections { get; set; } = new List<MemberCollection>();

            public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

            public virtual ICollection<TaskKeywordList> TaskKeywordLists { get; set; } = new List<TaskKeywordList>();

            public virtual ICollection<TaskPhoto> TaskPhotos { get; set; } = new List<TaskPhoto>();
        
    }
}
