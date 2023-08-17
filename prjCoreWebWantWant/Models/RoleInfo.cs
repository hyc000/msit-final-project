using System;
using System.Collections.Generic;

namespace prjCoreWebWantWant.Models;

public partial class RoleInfo
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<MemberRoleConn> MemberRoleConns { get; set; } = new List<MemberRoleConn>();
}
