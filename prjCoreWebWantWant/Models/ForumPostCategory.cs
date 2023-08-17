using System;
using System.Collections.Generic;

namespace prjCoreWebWantWant.Models;

public partial class ForumPostCategory
{
    public int PostCategoryId { get; set; }

    public int PostId { get; set; }

    public int CategoryId { get; set; }

    public virtual ForumCategory Category { get; set; } = null!;

    public virtual ForumPost Post { get; set; } = null!;
}
