using System;
using System.Collections.Generic;

namespace prjCoreWebWantWant.Models;

public partial class ForumPost
{
    public int PostId { get; set; }

    public int AccountId { get; set; }

    public int? ParentId { get; set; }

    public string? Title { get; set; }

    public string PostContent { get; set; } = null!;

    public DateTime Created { get; set; }

    public DateTime? Updated { get; set; }

    public byte Status { get; set; }

    public int? ViewCount { get; set; }

    public int? LikeCount { get; set; }

    public virtual MemberAccount Account { get; set; } = null!;

    public virtual ICollection<ForumPostCategory> ForumPostCategories { get; set; } = new List<ForumPostCategory>();

    public virtual ICollection<ForumPostComment> ForumPostComments { get; set; } = new List<ForumPostComment>();

    public virtual ICollection<ForumPostLike> ForumPostLikes { get; set; } = new List<ForumPostLike>();

    public virtual ICollection<ForumPost> InverseParent { get; set; } = new List<ForumPost>();

    public virtual ForumPost? Parent { get; set; }

    public virtual ForumStatus StatusNavigation { get; set; } = null!;
}
