using System;
using System.Collections.Generic;

namespace prjCoreWebWantWant.Models;

public partial class Certificate
{
    public int CertificateId { get; set; }

    public int CertificateTypeId { get; set; }

    public string? CertificateName { get; set; }

    public virtual CertificateType CertificateType { get; set; } = null!;

    public virtual ICollection<ResumeCertificate> ResumeCertificates { get; set; } = new List<ResumeCertificate>();
}
