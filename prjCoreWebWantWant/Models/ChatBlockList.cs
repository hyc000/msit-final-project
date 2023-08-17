using System;
using System.Collections.Generic;

namespace prjCoreWebWantWant.Models;

public partial class ChatBlockList
{
    public int ChatBlockListId { get; set; }

    public int AccountId { get; set; }

    public int BlockedId { get; set; }

    public virtual MemberAccount Account { get; set; } = null!;

    public virtual MemberAccount Blocked { get; set; } = null!;
}
