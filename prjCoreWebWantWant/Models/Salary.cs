using System;
using System.Collections.Generic;

namespace prjCoreWebWantWant.Models;

public partial class Salary
{
    public int SalaryId { get; set; }

    public int PaymentId { get; set; }

    public int PaymentDateId { get; set; }

    public virtual ICollection<TaskList> TaskLists { get; set; } = new List<TaskList>();
}
