using prjCoreWebWantWant.Models;

namespace prjCoreWebWantWant.ViewModels
{
    public class CIndexInfoViewModel
    {
        public TaskList taskList { get; set; }
        public Rating rating { get; set; }
        public ExpertResume expertResume { get; set; }
        public ExpertWorkList expertWorkList { get; set; }
        //Resume
        public Resume resume { get; set; }
        //MemberAccount
        public MemberAccount memberAccount { get; set; }
        public string townName
        {
            get
            {
                NewIspanProjectContext db = new NewIspanProjectContext();
                string name = db.Towns.Where(x => x.TownId == this.resume.TownId).Select(x => x.Town1).FirstOrDefault();
                return name;
            }
        }
        public string cityName
        {
            get
            {
                NewIspanProjectContext db = new NewIspanProjectContext();
                string cityNameList = db.Towns.Where(x => x.TownId == this.resume.TownId).Select(x => x.City.City1).FirstOrDefault();

                return cityNameList;
            }
        }
    }
}
