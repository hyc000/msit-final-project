using DocumentFormat.OpenXml.Wordprocessing;
using prjCoreWebWantWant.Models;
using System.ComponentModel.DataAnnotations;

namespace WantTask.ViewModels
{
    public class CApproveViewModel
    {
        public int ? CaseId { get; set; }
        public int ? ResumeId { get; set; }
        public int ? TaskNameId { get; set; }
        public int ? CaseStatusId { get; set; }
        public string ? Name { get; set; }
        public string ? SkillName { get; set; }
        public string ? CertificateName { get; set; }
        public string ? Autobiography { get; set; }
        public string ? PublishStart { get; set; }
        public string ? TaskStart { get; set; }
        public string ? TaskTitle { get; set; }
        public string ? TaskDetail { get; set; }        
        public string? TaskName { get; set; }
        public List<TaskList>? taskList { get; set; }       
        //public List<ApplicationList>? applicationList { get; set; }
        public List<CaseStatusList>? caseStatusList { get; set; }
        //public List<MemberAccount>? memberAccount { get; set; }

        public List<Resume>? resume { get; set; }

        // public List<Certificate>? certificate { get; set; }
        // public List<ResumeCertificate>? resumeCertificate { get; set; }

        //public List<Skill>? skill { get; set; }
        // public List<ResumeSkill>? resumeSkill  { get; set; }

        //該履歷擁有的技能和證照
        public List<string> SkillNames { get; set; }
        public List<string> CertificateNames { get; set; }

        //我發案的工作所需的技能和證照
        public List<string> RequiredSkills { get; set; } // 工作所需的技能
        public List<string> RequiredCertificates { get; set; } // 工作所需的證書


        // 吻合度:使用 double 來表示百分比（0.0 到 1.0）
        public double MatchingScore { get; set; }

        // 吻合度:添加一個只讀屬性來表示百分比的字串
        public string MatchingScorePercentage => (MatchingScore * 100).ToString("0.00") + "%";


    }
}
