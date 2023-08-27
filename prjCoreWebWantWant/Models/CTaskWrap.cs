using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace prjCoreWebWantWant.Models;

    public class CTaskWrap
    {
        private TaskList _task = null;

        private TaskNameList _taskNameList = null;

    //private TaskPhoto _taskPhoto = null;


    public TaskList task
            {
                get { return _task; }
                set { _task = value; }
            }

        //public TaskNameList taskNameList
        //{
        //    get { return _taskNameList; }
        //    set { _taskNameList = value; }
        //}

    //public TaskPhoto taskPhoto
    //{
    //    get { return _taskPhoto; }
    //    set { _taskPhoto = value; }
    //}

    public TaskPhoto taskPhoto { get; set; }
    //public TaskNameList taskNameList { get; set; }

    public CTaskWrap()
            {
                _task = new TaskList();
              
        _taskNameList= new TaskNameList();
        //_taskPhoto= new TaskPhoto();
    }

        public int FId
        {
            get { return _task.CaseId; }
            set { _task.CaseId = value; }
        }
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
        public int? FTownId
        {
            get { return _task.TownId; }
            set { _task.TownId = value; }
        }
        public string? FAddress
        {
            get { return _task.Address; }
            set { _task.Address = value; }
        }
        public string? FRequiredNum
        {
            get { return _task.RequiredNum; }
            set { _task.RequiredNum = value; }
        }
        public string? FTaskPeriod
        {
            get { return _task.TaskPeriod; }
            set { _task.TaskPeriod = value; }
        }

    public string? FTaskStartDate
    {
        get { return _task.TaskStartDate; }
        set { _task.TaskStartDate = value; }
    }

    public string? FTaskEndDate
    {
        get { return _task.TaskEndDate; }
        set { _task.TaskEndDate = value; }
    }
    public string? FTaskStartHour
        {
            get { return _task.TaskStartHour; }
            set { _task.TaskStartHour = value; }
        }
        public string? FTaskEndHour
    {
            get { return _task.TaskEndHour; }
            set { _task.TaskEndHour = value; }
        }
    public string? FPublishOrNot
        {    
            get { return _task.PublishOrNot; }
            set { _task.PublishOrNot = value; }
        }
        public string? FPublishStart
        {
            get { return _task.PublishStart; }
            set { _task.PublishStart = value; }
        }
        public string? FPublishEnd
        {
            get { return _task.PublishEnd; }
            set { _task.PublishEnd = value; }
        }

        public int? FAccountID
        {
            get { return _task.AccountId; }
            set { _task.AccountId = value; }
        }

    public string? FTaskName
    {
        get { return _taskNameList.TaskName; }
        set { _taskNameList.TaskName = value; }
    }




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

