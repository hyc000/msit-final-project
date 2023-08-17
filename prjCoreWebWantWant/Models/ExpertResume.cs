using System;
using System.Collections.Generic;

namespace prjCoreWebWantWant.Models;

public partial class ExpertResume
{
    public int ResumeId { get; set; }

    public string? Introduction { get; set; }

    public string? WorksUrl { get; set; }

    public string? ContactMethod { get; set; }

    public string? ServiceMethod { get; set; }

    public string? PaymentMethod { get; set; }

    public string? Problem { get; set; }

    public decimal? CommonPrice { get; set; }

    public int? HistoricalViews { get; set; }

    public virtual ICollection<ExpertWorkList> ExpertWorkLists { get; set; } = new List<ExpertWorkList>();

    public virtual Resume Resume { get; set; } = null!;
}
