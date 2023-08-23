using prjCoreWebWantWant.Models;

namespace prjCoreWebWantWant.ViewModels
{
    public class CBackstageManagementViewModel
    { 
        public int AccountId { get; set; }

        public string Email { get; set; } = null!;
       
        public bool? AccountStatus { get; set; }              

        public DateTime CreateTime { get; set; }               

        public string? Name { get; set; }           
      
        public string? PhoneNo { get; set; }

        public MemberStatusList memberStatusList { get; set; }

        public LoginHistory loginHistory { get; set; }
    }
}
