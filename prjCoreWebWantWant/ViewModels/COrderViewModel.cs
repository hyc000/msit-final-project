using prjCoreWebWantWant.Models;

namespace prjCoreWebWantWant.ViewModels
{
    public class COrderViewModel
    {
        public List<TaskList> TaskLists { get; set; }
        public List<Resume> Resumes { get; set; }
        public List<Order> Orders { get; set; }
    }
}
