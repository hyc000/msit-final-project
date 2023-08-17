using System;
using System.Collections.Generic;

namespace prjCoreWebWantWant.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public string? Payment1 { get; set; }

    public virtual ICollection<TaskList> TaskLists { get; set; } = new List<TaskList>();
}
