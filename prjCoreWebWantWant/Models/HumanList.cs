using System;
using System.Collections.Generic;

namespace prjCoreWebWantWant.Models;

public partial class HumanList
{
    public int? CaseId { get; set; }

    public int HumanListId { get; set; }

    public int? CaseStatusId { get; set; }

    public int? ApplicantId { get; set; }

    public int? SenderId { get; set; }
}
