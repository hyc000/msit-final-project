using System;
using System.Collections.Generic;

namespace prjCoreWebWantWant.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int? AccountId { get; set; }

    public int? PayWayId { get; set; }

    public int? CategoryId { get; set; }

    public int? StatusId { get; set; }

    public int? OrderPrice { get; set; }

    public int? OrderPoint { get; set; }

    public DateTime? CreateTime { get; set; }

    public int? OrderGetPoint { get; set; }

    public int? OrderUsePoint { get; set; }

    public virtual MemberAccount? Account { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual PayWay? PayWay { get; set; }
}
