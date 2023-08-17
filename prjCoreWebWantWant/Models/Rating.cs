using System;
using System.Collections.Generic;

namespace prjCoreWebWantWant.Models;

public partial class Rating
{
    public int RatingId { get; set; }

    public string RatingStar { get; set; } = null!;

    public string? RatingContent { get; set; }

    public DateTime? RatingDate { get; set; }

    public int? SourceRoleId { get; set; }

    public int? SourceAccountId { get; set; }

    public int? TargetRoleId { get; set; }

    public int? TargetAccountId { get; set; }

    public virtual ICollection<ApplicationList> ApplicationLists { get; set; } = new List<ApplicationList>();

    public virtual ICollection<ExpertApplication> ExpertApplications { get; set; } = new List<ExpertApplication>();
}
