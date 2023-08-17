using System;
using System.Collections.Generic;

namespace prjCoreWebWantWant.Models;

public partial class ForumPostComment
{
    public int PostCommentId { get; set; }

    public int PostId { get; set; }

    public int? ParentId { get; set; }

    public int AccountId { get; set; }

    public string Comment { get; set; } = null!;

    public DateTime Created { get; set; }

    public DateTime? Updated { get; set; }

    public byte Status { get; set; }

    public virtual MemberAccount Account { get; set; } = null!;

    public virtual ICollection<ForumPostComment> InverseParent { get; set; } = new List<ForumPostComment>();

    public virtual ForumPostComment? Parent { get; set; }

    public virtual ForumPost Post { get; set; } = null!;

    public virtual ForumStatus StatusNavigation { get; set; } = null!;
}
