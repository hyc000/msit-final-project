using prjCoreWebWantWant.Models;

namespace prjCoreWebWantWant.ViewModels
{
    public class CBackstageViewModel
    {
        public TaskList taskList { get; set; }
        public ExpertResume expertResume { get; set; }
        public ExpertWorkList expertWorkList { get; set; }
        //Resume
        public Resume resume { get; set; }
        //MemberAccount
        public MemberAccount memberAccount { get; set; }
    }
}
