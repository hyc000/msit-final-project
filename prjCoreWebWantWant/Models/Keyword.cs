using System;
using System.Collections.Generic;

namespace prjCoreWebWantWant.Models;

public partial class Keyword
{
    public int KeywordId { get; set; }

    public string? KeywordContent { get; set; }

    public virtual ICollection<ResumeKeywordList> ResumeKeywordLists { get; set; } = new List<ResumeKeywordList>();

    public virtual ICollection<TaskKeywordList> TaskKeywordLists { get; set; } = new List<TaskKeywordList>();
}
