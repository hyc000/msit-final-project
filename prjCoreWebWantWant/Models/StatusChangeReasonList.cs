using System;
using System.Collections.Generic;

namespace prjCoreWebWantWant.Models;

public partial class StatusChangeReasonList
{
    public int StatusChangeReasonId { get; set; }

    public string? StatusChangeReason { get; set; }

    public virtual ICollection<MemberStatusList> MemberStatusLists { get; set; } = new List<MemberStatusList>();
}
