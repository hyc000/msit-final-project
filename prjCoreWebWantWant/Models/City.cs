using System;
using System.Collections.Generic;

namespace prjCoreWebWantWant.Models;

public partial class City
{
    public int CityId { get; set; }

    public string? City1 { get; set; }

    public virtual ICollection<Town> Towns { get; set; } = new List<Town>();
}
