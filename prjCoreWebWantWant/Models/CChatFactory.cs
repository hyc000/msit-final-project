using DocumentFormat.OpenXml.InkML;

namespace prjCoreWebWantWant.Models
{
    public class CChatFactory
    {
        private readonly NewIspanProjectContext _db;

        public CChatFactory(NewIspanProjectContext context)
        {
            _db = context;
        }

        public string UserAvatar(int id)
        {
            var ava = _db.MemberAccounts.Where(p => p.AccountId == id).Select(p => p.MemberPhoto).FirstOrDefault();
            byte[] userAvatarBytes = ava;
            string userAvatarBase64 = Convert.ToBase64String(userAvatarBytes);
            string userAvatarUrl = $"data:image/png;base64,{userAvatarBase64}";
            return userAvatarUrl;
        }

        public string GetUserName(int id)
        {
            var name = _db.MemberAccounts.Find(id).UserName.ToString();
            return name;
        }

    }
}
