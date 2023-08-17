using System;
using System.Collections.Generic;

namespace prjCoreWebWantWant.Models;

public partial class ExpertApplication
{
    public int ApplicationListId { get; set; }

    public int? CaseId { get; set; }

    public int? AccountId { get; set; }

    public int? CaseStatusId { get; set; }

    public int? RatingId { get; set; }

    public int? ExpertAccountId { get; set; }

    public virtual MemberAccount? Account { get; set; }

    public virtual TaskList? Case { get; set; }

    public virtual CaseStatusList? CaseStatus { get; set; }

    public virtual Rating? Rating { get; set; }
}
