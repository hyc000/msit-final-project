using System;
using System.Collections.Generic;

namespace prjCoreWebWantWant.Models;

public partial class ExpertWork
{
    public int WorksId { get; set; }

    public byte[]? WorksPhoto { get; set; }

    public DateTime? DataCreateDate { get; set; }

    public string? Workname { get; set; }

    public virtual ICollection<ExpertWorkList> ExpertWorkLists { get; set; } = new List<ExpertWorkList>();
}
