namespace prjCoreWebWantWant.Models
{
    public class CLoginUser //於session中存入本次登入資料&寫入登入紀錄資料表
    {
       
        public int AccountId { get;set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int UserRole
        {
            get
            {
                NewIspanProjectContext db = new NewIspanProjectContext();
                int UserRole = (from memRole in db.MemberRoleConns
                                 where memRole.AccountId == this.AccountId
                                 select memRole.RoleId).FirstOrDefault();
                return UserRole;
            }
        }
        public int LastFailCount
        {
            get
            {
                NewIspanProjectContext db = new NewIspanProjectContext();
                int LastFailCount = (from loginHis in db.LoginHistories
                                 where loginHis.AccountId == this.AccountId
                                 orderby loginHis.LoginId descending
                                 select loginHis.PasswordFailCount).FirstOrDefault();
                return LastFailCount;
            }

        }
        public static Boolean memInfoFinished
        {
            get
            {
                return true;
            }
        }

        public static void setLogHis(LoginHistory loghis,int id, int lastFailC, int failcount, Boolean loginsf)
        {
            loghis.AccountId = id;
            loghis.LoginSF = loginsf;
            loghis.LoginTime = DateTime.Now;
            if (failcount == 0)
            {
                loghis.PasswordFailCount = 0;
            }
            else
            {
                loghis.PasswordFailCount = failcount + lastFailC;
            }
            loghis.MemberInfoFinished = CLoginUser.memInfoFinished;
        }

    }
    
}
