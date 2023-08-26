namespace prjCoreWebWantWant.Models
{
    public class CResumeWrap
    {
        private Resume _resume = null;
        public Resume resume
        {
            get { return _resume; }
            set { _resume = value; }
        }
        public CResumeWrap()
        {
            _resume = new Resume();
        }

        public int FId
        {
            get { return _resume.ResumeId; }
            set { _resume.ResumeId = value; }
        }
        public string? FTitle
        {
            get { return _resume.ResumeTitle; }
            set { _resume.ResumeTitle = value; }
        }
        public string? FAutobiography
        {
            get { return _resume.Autobiography; }
            set { _resume.Autobiography = value; }
        }
        public string? Address
        {
            get { return _resume.Address; }
            set { _resume.Address = value; }
        }
        public int? FTownId
        {
            get { return _resume.TownId; }
            set { _resume.TownId = value; }
        }
        public string? FAddress
        {
            get { return _resume.Address; }
            set { _resume.Address = value; }
        }


        public virtual ICollection<TaskNameList> taskNameList { get; set; } = new List<TaskNameList>();
        public virtual ICollection<TaskSkill> taskSkill { get; set; } = new List<TaskSkill>();
        public virtual ICollection<TaskCertificate> taskCertificate { get; set; } = new List<TaskCertificate>();
    }
}
