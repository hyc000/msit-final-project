using System;
using System.Collections.Generic;

namespace prjCoreWebWantWant.Models;

public partial class ForumStatus
{
    public byte StatusId { get; set; }

    public string StatusName { get; set; } = null!;

    public virtual ICollection<ForumPostComment> ForumPostComments { get; set; } = new List<ForumPostComment>();

    public virtual ICollection<ForumPost> ForumPosts { get; set; } = new List<ForumPost>();
}
