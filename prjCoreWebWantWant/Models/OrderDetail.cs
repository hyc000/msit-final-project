using System;
using System.Collections.Generic;

namespace prjCoreWebWantWant.Models;

public partial class OrderDetail
{
    public int OrderdetailId { get; set; }

    public int? OrderId { get; set; }

    public int? ProductId { get; set; }

    public int? CaseId { get; set; }

    public int? ResumeId { get; set; }

    public int? UnitPrice { get; set; }

    public int? Quantity { get; set; }

    public float? Discount { get; set; }

    public int? TopDate { get; set; }

    public string? TopType { get; set; }

    public int? UnitPoint { get; set; }

    public int? GetPoint { get; set; }

    public virtual TaskList? Case { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Product? Product { get; set; }

    public virtual Resume? Resume { get; set; }
}
