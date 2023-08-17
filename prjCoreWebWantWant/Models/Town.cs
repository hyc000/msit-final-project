using System;
using System.Collections.Generic;

namespace prjCoreWebWantWant.Models;

public partial class Town
{
    public int TownId { get; set; }

    public int CityId { get; set; }

    public string Town1 { get; set; } = null!;

    public int PostalCode { get; set; }

    public virtual City City { get; set; } = null!;

    public virtual ICollection<Resume> Resumes { get; set; } = new List<Resume>();

    public virtual ICollection<TaskList> TaskLists { get; set; } = new List<TaskList>();
}
