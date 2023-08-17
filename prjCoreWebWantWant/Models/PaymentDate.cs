using System;
using System.Collections.Generic;

namespace prjCoreWebWantWant.Models;

public partial class PaymentDate
{
    public int PaymentDateId { get; set; }

    public string? PaymentDate1 { get; set; }

    public virtual ICollection<TaskList> TaskLists { get; set; } = new List<TaskList>();
}
