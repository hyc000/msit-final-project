using System;
using System.Collections.Generic;

namespace prjCoreWebWantWant.Models;

public partial class LoginHistory
{
    public int LoginId { get; set; }

    public int AccountId { get; set; }

    public bool LoginSF { get; set; }

    public DateTime LoginTime { get; set; }

    public int PasswordFailCount { get; set; }

    public bool MemberInfoFinished { get; set; }

    public virtual MemberAccount Account { get; set; } = null!;
}
