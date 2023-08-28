using prjCoreWebWantWant.Models;

namespace prjCoreWebWantWant.ViewModels
{
    public class OrderListViewModel
    {
        public List<Order> Orders { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
    }
}
