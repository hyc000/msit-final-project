using System;
using System.Collections.Generic;

namespace prjCoreWebWantWant.Models;

public partial class CheckIn
{
    public int CheckId { get; set; }

    public int AccountId { get; set; }

    public DateTime CheckDate { get; set; }

    public int CheckPoint { get; set; }

    public virtual MemberAccount Account { get; set; } = null!;
}
