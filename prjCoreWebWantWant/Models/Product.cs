using System;
using System.Collections.Generic;

namespace prjCoreWebWantWant.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public int? CategoryId { get; set; }

    public string? ProductName { get; set; }

    public int? UnitPrice { get; set; }

    public string? ProductDesc { get; set; }

    public string? Status { get; set; }

    public DateTime? PostStartDate { get; set; }

    public DateTime? PostEndDate { get; set; }

    public int? UnitsInStock { get; set; }

    public int? TopDate { get; set; }

    public string? TopType { get; set; }

    public int? UnitPoint { get; set; }

    public int? GetPoint { get; set; }

    public string? CoverPhoto { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<ProductPhoto> ProductPhotos { get; set; } = new List<ProductPhoto>();
}
