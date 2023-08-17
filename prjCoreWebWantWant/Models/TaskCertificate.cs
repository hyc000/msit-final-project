using System;
using System.Collections.Generic;

namespace prjCoreWebWantWant.Models;

public partial class TaskCertificate
{
    public int TaskCertificateId { get; set; }

    public int CaseId { get; set; }

    public int CertficateId { get; set; }

    public virtual TaskList Case { get; set; } = null!;
}
