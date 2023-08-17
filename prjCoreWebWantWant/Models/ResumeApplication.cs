using System;
using System.Collections.Generic;

namespace prjCoreWebWantWant.Models;

public partial class ResumeApplication
{
    public int ResumeApplicationId { get; set; }

    public int? ResumeId { get; set; }

    public int? CaseId { get; set; }

    public int? CaseStatusId { get; set; }

    public virtual Resume? Resume { get; set; }
}
