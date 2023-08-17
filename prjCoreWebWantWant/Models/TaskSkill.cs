using System;
using System.Collections.Generic;

namespace prjCoreWebWantWant.Models;

public partial class TaskSkill
{
    public int TaskSkillId { get; set; }

    public int CaseId { get; set; }

    public int SkillId { get; set; }

    public virtual TaskList Case { get; set; } = null!;
}
