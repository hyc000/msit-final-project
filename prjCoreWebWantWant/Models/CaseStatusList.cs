using System;
using System.Collections.Generic;

namespace prjCoreWebWantWant.Models;

public partial class CaseStatusList
{
    public int CaseStatusId { get; set; }

    public string? CaseStatus { get; set; }

    public virtual ICollection<ApplicationList> ApplicationLists { get; set; } = new List<ApplicationList>();

    public virtual ICollection<ExpertApplication> ExpertApplications { get; set; } = new List<ExpertApplication>();

    public virtual ICollection<Resume> Resumes { get; set; } = new List<Resume>();
}
