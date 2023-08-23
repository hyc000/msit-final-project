using prjCoreWebWantWant.Models;

namespace prjCoreWebWantWant.ViewModels
{
    public class CServiceContactViewModel
    {
        public ServiceContact serviceContact { get; set; }
        public int AccountId { get; set; }
        public string Email { get; set; }
        public string? Name { get; set; }
        public string? PhoneNo { get; set; }
    }
}
