namespace prjCoreWebWantWant.Models
{
    public class CCartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }
        public int? GetPoint { get; set; }
        public int? CategoryId { get; set; }
        public int? TopDate { get; set; }
    }
}
