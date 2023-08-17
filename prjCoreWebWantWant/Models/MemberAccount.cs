using System;
using System.Collections.Generic;

namespace prjCoreWebWantWant.Models;

public partial class MemberAccount
{
    public int AccountId { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool? AccountStatus { get; set; }

    public int MemberTotalPoint { get; set; }

    public DateTime CreateTime { get; set; }

    public string? IdcardNo { get; set; }

    public string? Name { get; set; }

    public string? Gender { get; set; }

    public string? UserName { get; set; }

    public DateTime? BitrhDay { get; set; }

    public string? PhoneNo { get; set; }

    public byte[]? MemberPhoto { get; set; }

    public virtual ICollection<ChatBlockList> ChatBlockListAccounts { get; set; } = new List<ChatBlockList>();

    public virtual ICollection<ChatBlockList> ChatBlockListBlockeds { get; set; } = new List<ChatBlockList>();

    public virtual ICollection<ChatMessage> ChatMessageReceivers { get; set; } = new List<ChatMessage>();

    public virtual ICollection<ChatMessage> ChatMessageSenders { get; set; } = new List<ChatMessage>();

    public virtual ICollection<CheckIn> CheckIns { get; set; } = new List<CheckIn>();

    public virtual ICollection<ExpertApplication> ExpertApplications { get; set; } = new List<ExpertApplication>();

    public virtual ICollection<ForgetPassword> ForgetPasswords { get; set; } = new List<ForgetPassword>();

    public virtual ICollection<ForumPostComment> ForumPostComments { get; set; } = new List<ForumPostComment>();

    public virtual ICollection<ForumPost> ForumPosts { get; set; } = new List<ForumPost>();

    public virtual ICollection<LoginHistory> LoginHistories { get; set; } = new List<LoginHistory>();

    public virtual ICollection<MemberCollection> MemberCollections { get; set; } = new List<MemberCollection>();

    public virtual ICollection<MemberRoleConn> MemberRoleConns { get; set; } = new List<MemberRoleConn>();

    public virtual ICollection<MemberStatusList> MemberStatusLists { get; set; } = new List<MemberStatusList>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Resume> Resumes { get; set; } = new List<Resume>();

    public virtual ICollection<ServiceContact> ServiceContacts { get; set; } = new List<ServiceContact>();
}
