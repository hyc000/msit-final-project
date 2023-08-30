using prjCoreWebWantWant.ViewModels;

namespace prjCoreWebWantWant.Models
{
    public class CExperTaskFactory
    {
        private readonly NewIspanProjectContext _context;
     
        public CExperTaskFactory(NewIspanProjectContext context)
        {
            _context = context;
           
        
        }

        //Member轉換器 ID變成名字
        public string MemberName(int? MemberID)
        {
            if (MemberID != null)
            {
                string memberName = _context.MemberAccounts
                .Where(x => x.AccountId == MemberID)
                .Select(x => x.Name)
                .FirstOrDefault();
                return memberName;
            }

            return null;
        }
        public int? MemberID(string? membername)
        {
            if (membername != null)
            {
                int memberid = _context.MemberAccounts
                .Where(x => x.Name == membername)
                .Select(x => x.AccountId)
                .FirstOrDefault();
                return memberid;
            }

            return null;
        }

        public string MemberEmail(int? MemberID)
        {
            if (MemberID != null)
            {
                string memberEmail = _context.MemberAccounts
                .Where(x => x.AccountId == MemberID)
                .Select(x => x.Email)
                .FirstOrDefault();
                return memberEmail;
            }

            return null;
        }



        public string StatusName(int? StatusID)
        {
            if (StatusID != null)
            {
                string statusName = _context.CaseStatusLists
                .Where(x => x.CaseStatusId == StatusID)
                .Select(x => x.CaseStatus)
                .FirstOrDefault();
                return statusName;
            }

            return null;
        }

        //發案用
        public string TaskName(int? tasknameID)
        {
            if (tasknameID != null)
            {
                string taskName = _context.TaskNameLists
                .Where(x => x.TaskNameId == tasknameID)
                .Select(x => x.TaskName)
                .FirstOrDefault();
                return taskName;
            }

            return null;
        }

        public int? TownName(string? townname)
        {
            if (townname != null)
            {
                int townid = _context.Towns
                .Where(x => x.Town1 == townname)
                .Select(x => x.TownId)
                .FirstOrDefault();
                return townid;
            }

            return null;
        }


        public int? SkillName(string? skillname)
        {
            if (skillname != null)
            {
                int skillid = _context.Skills
                .Where(x => x.SkillName== skillname)
                .Select(x => x.SkillId)
                .FirstOrDefault();
                return skillid;
            }

            return null;
        }

        public int? CertificateName(string? certificatename)
        {
            if (certificatename != null)
            {
                int certificateid = _context.Certificates
                .Where(x => x.CertificateName == certificatename)
                .Select(x => x.CertificateId)
                .FirstOrDefault();
                return certificateid;
            }

            return null;
        }


    }
}
