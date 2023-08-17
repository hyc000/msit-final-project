using System;
using System.Collections.Generic;

namespace prjCoreWebWantWant.Models;

public partial class Skill
{
    public int SkillId { get; set; }

    public int SkillTypeId { get; set; }

    public string? SkillName { get; set; }

    public virtual ICollection<CaseSkill> CaseSkills { get; set; } = new List<CaseSkill>();

    public virtual ICollection<ResumeSkill> ResumeSkills { get; set; } = new List<ResumeSkill>();

    public virtual SkillType SkillType { get; set; } = null!;
}
