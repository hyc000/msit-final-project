
using prjCoreWebWantWant.Models;

namespace prjCoreWantMember.ViewModels
{
    public class CExpertInfoViewModel
    {

        public ExpertResume expertResume { get; set; }



        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ExpertWorkList expertWorkList { get; set; }


        //Resume
        public Resume resume { get; set; }



        public string townName
        {
            get
            {
                NewIspanProjectContext db = new NewIspanProjectContext();
                string name = db.Towns.Where(x => x.TownId == this.resume.TownId).Select(x => x.Town1).FirstOrDefault();
                return name;
            }
        }
        private string FindCity()
        {
            NewIspanProjectContext db = new NewIspanProjectContext();
            var cityNameList = db.Towns
           .Where(x => x.TownId == this.resume.TownId)
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



        //MemberAccount
        public MemberAccount memberAccount { get; set; }

    }
}
