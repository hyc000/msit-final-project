using prjCoreWebWantWant.Models;

namespace prjCoreWebWantWant.ViewModels
{
    public class CServiceContactViewModel
    {
        public int ServiceContactId { get; set; }

        public int AccountId { get; set; }

        public string? ComplaintTitle { get; set; }

        public string? ComplaintContent { get; set; }

        public bool? ProcessStatus { get; set; }

        public MemberAccount memberAccount { get; set; }
    }
}
