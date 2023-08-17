using System;
using System.Collections.Generic;

namespace prjCoreWebWantWant.Models;

public partial class ApplicationList
{
    public int? ResumeId { get; set; }

    public int ApplicationListId { get; set; }

    public int? CaseId { get; set; }

    public int? CaseStatusId { get; set; }

    public int? RatingId { get; set; }

    public virtual TaskList? Case { get; set; }

    public virtual CaseStatusList? CaseStatus { get; set; }

    public virtual Rating? Rating { get; set; }

    public virtual Resume? Resume { get; set; }
}
