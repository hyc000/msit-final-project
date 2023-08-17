using System;
using System.Collections.Generic;

namespace prjCoreWebWantWant.Models;

public partial class ServiceContact
{
    public int ServiceContactId { get; set; }

    public int AccountId { get; set; }

    public string? ComplaintTitle { get; set; }

    public string? ComplaintContent { get; set; }

    public bool? ProcessStatus { get; set; }

    public virtual MemberAccount Account { get; set; } = null!;
}
