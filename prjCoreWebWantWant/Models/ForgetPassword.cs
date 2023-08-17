using System;
using System.Collections.Generic;

namespace prjCoreWebWantWant.Models;

public partial class ForgetPassword
{
    public int ForgetId { get; set; }

    public int AccountId { get; set; }

    public string TemporaryPassword { get; set; } = null!;

    public int ForgetCountInAweek { get; set; }

    public DateTime CreateTime { get; set; }

    public virtual MemberAccount Account { get; set; } = null!;
}
