using System;
using System.Collections.Generic;

namespace prjCoreWebWantWant.Models;

public partial class MemberCollection
{
    public int CollectionId { get; set; }

    public int AccountId { get; set; }

    public int? CaseId { get; set; }

    public int? ResumeId { get; set; }

    public virtual MemberAccount Account { get; set; } = null!;

    public virtual TaskList? Case { get; set; }

    public virtual Resume? Resume { get; set; }
}
