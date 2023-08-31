using prjCoreWebWantWant.Models;

namespace prjCoreWebWantWant.ViewModels
{
    public class CExpertSearchViewModel
    {
        //MemberAccount
        public int AccountId { get; set; }
        public string Name { get; set; }
        //Resume
        public int ResumeId { get; set; }
        public string ResumeTitle { get; set; }
        public string DataModifyDate { get; set; }
        public byte[] Photo { get; set; }
        public int? TownId { get; set; }

        //ExpertResume
        public string? Introduction { get; set; }
        public string? ContactMethod { get; set; }
        public string? PaymentMethod { get; set; }
        public decimal? CommonPrice { get; set; }
        public int? HistoricalViews { get; set; }
        //證照&專長要整張表
        public string? SkillNames { get; set; }
        public string? CertificateNames { get; set; }

        //方法
        public string townName
        {
            get
            {
                NewIspanProjectContext db = new NewIspanProjectContext();
                string name = db.Towns.Where(x => x.TownId == this.TownId).Select(x => x.Town1).FirstOrDefault();
                return name;
            }
        }
        private string FindCity()
        {
            NewIspanProjectContext db = new NewIspanProjectContext();
            var cityNameList = db.Towns
           .Where(x => x.TownId == this.TownId)
           .Select(x => x.City.City1)
           .FirstOrDefault();

            return cityNameList;
        }


        public string cityName
        {
            get
            {
                return this.FindCity();

            }
        }
     
    }
}
