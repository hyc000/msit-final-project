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

    }
}
