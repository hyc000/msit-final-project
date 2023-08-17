using System;
using System.Collections.Generic;

namespace prjCoreWebWantWant.Models;

public partial class CaseSkill
{
    public int CaseSkillId { get; set; }

    public int? SkillId { get; set; }

    public int? CaseId { get; set; }

    public virtual TaskList? Case { get; set; }

    public virtual Skill? Skill { get; set; }
}
