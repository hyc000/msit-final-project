using System;
using System.Collections.Generic;

namespace prjCoreWebWantWant.Models;

public partial class Resume
{
    public int AccountId { get; set; }

    public int ResumeId { get; set; }

    public int? TownId { get; set; }

    public string? Address { get; set; }

    public string? Autobiography { get; set; }

    public int? WorkingHoursId { get; set; }

    public byte[]? Photo { get; set; }

    public int? OnTop { get; set; }

    public int? TaskNameId { get; set; }

    public bool IsExpertOrNot { get; set; }

    public int? CaseStatusId { get; set; }

    public virtual MemberAccount Account { get; set; } = null!;

    public virtual ICollection<ApplicationList> ApplicationLists { get; set; } = new List<ApplicationList>();

    public virtual CaseStatusList? CaseStatus { get; set; }

    public virtual ExpertResume? ExpertResume { get; set; }

    public virtual ICollection<MemberCollection> MemberCollections { get; set; } = new List<MemberCollection>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<ResumeCertificate> ResumeCertificates { get; set; } = new List<ResumeCertificate>();

    public virtual ICollection<ResumeKeywordList> ResumeKeywordLists { get; set; } = new List<ResumeKeywordList>();

    public virtual ICollection<ResumeSkill> ResumeSkills { get; set; } = new List<ResumeSkill>();

    public virtual TaskNameList? TaskName { get; set; }

    public virtual Town? Town { get; set; }

    public virtual WorkingTime? WorkingHours { get; set; }
}
