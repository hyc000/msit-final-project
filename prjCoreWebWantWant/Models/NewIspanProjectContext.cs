using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace prjCoreWebWantWant.Models;

public partial class NewIspanProjectContext : DbContext
{
    public NewIspanProjectContext()
    {
    }

    public NewIspanProjectContext(DbContextOptions<NewIspanProjectContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ApplicationList> ApplicationLists { get; set; }

    public virtual DbSet<CaseSkill> CaseSkills { get; set; }

    public virtual DbSet<CaseStatusList> CaseStatusLists { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Certificate> Certificates { get; set; }

    public virtual DbSet<CertificateType> CertificateTypes { get; set; }

    public virtual DbSet<ChatBlockList> ChatBlockLists { get; set; }

    public virtual DbSet<ChatMessage> ChatMessages { get; set; }

    public virtual DbSet<CheckIn> CheckIns { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<ExpertApplication> ExpertApplications { get; set; }

    public virtual DbSet<ExpertResume> ExpertResumes { get; set; }

    public virtual DbSet<ExpertWork> ExpertWorks { get; set; }

    public virtual DbSet<ExpertWorkList> ExpertWorkLists { get; set; }

    public virtual DbSet<ForgetPassword> ForgetPasswords { get; set; }

    public virtual DbSet<ForumCategory> ForumCategories { get; set; }

    public virtual DbSet<ForumPost> ForumPosts { get; set; }

    public virtual DbSet<ForumPostCategory> ForumPostCategories { get; set; }

    public virtual DbSet<ForumPostComment> ForumPostComments { get; set; }

    public virtual DbSet<ForumPostLike> ForumPostLikes { get; set; }

    public virtual DbSet<ForumStatus> ForumStatuses { get; set; }

    public virtual DbSet<HumanList> HumanLists { get; set; }

    public virtual DbSet<Keyword> Keywords { get; set; }

    public virtual DbSet<LoginHistory> LoginHistories { get; set; }

    public virtual DbSet<MemberAccount> MemberAccounts { get; set; }

    public virtual DbSet<MemberCollection> MemberCollections { get; set; }

    public virtual DbSet<MemberRoleConn> MemberRoleConns { get; set; }

    public virtual DbSet<MemberStatusList> MemberStatusLists { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<PayWay> PayWays { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PaymentDate> PaymentDates { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductPhoto> ProductPhotos { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<Resume> Resumes { get; set; }

    public virtual DbSet<ResumeCertificate> ResumeCertificates { get; set; }

    public virtual DbSet<ResumeKeywordList> ResumeKeywordLists { get; set; }

    public virtual DbSet<ResumeSkill> ResumeSkills { get; set; }

    public virtual DbSet<RoleInfo> RoleInfos { get; set; }

    public virtual DbSet<Salary> Salaries { get; set; }

    public virtual DbSet<ServiceContact> ServiceContacts { get; set; }

    public virtual DbSet<Skill> Skills { get; set; }

    public virtual DbSet<SkillType> SkillTypes { get; set; }

    public virtual DbSet<StatusChangeReasonList> StatusChangeReasonLists { get; set; }

    public virtual DbSet<TaskCertificate> TaskCertificates { get; set; }

    public virtual DbSet<TaskKeywordList> TaskKeywordLists { get; set; }

    public virtual DbSet<TaskList> TaskLists { get; set; }

    public virtual DbSet<TaskNameList> TaskNameLists { get; set; }

    public virtual DbSet<TaskPhoto> TaskPhotos { get; set; }

    public virtual DbSet<TaskSkill> TaskSkills { get; set; }

    public virtual DbSet<Town> Towns { get; set; }

    public virtual DbSet<WorkingTime> WorkingTimes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=124.219.117.33;Initial Catalog=NewIspanProject;Persist Security Info=True;User ID=msit150;Password=aaaa;Multiple Active Result Sets=True;Trust Server Certificate=True;Application Name=EntityFramework");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApplicationList>(entity =>
        {
            entity.ToTable("ApplicationList");

            entity.Property(e => e.ApplicationListId).HasColumnName("ApplicationListID");
            entity.Property(e => e.CaseId).HasColumnName("CaseID");
            entity.Property(e => e.CaseStatusId).HasColumnName("CaseStatusID");
            entity.Property(e => e.RatingId).HasColumnName("RatingID");
            entity.Property(e => e.ResumeId).HasColumnName("ResumeID");

            entity.HasOne(d => d.Case).WithMany(p => p.ApplicationLists)
                .HasForeignKey(d => d.CaseId)
                .HasConstraintName("FK_ApplicationList_TaskList");

            entity.HasOne(d => d.CaseStatus).WithMany(p => p.ApplicationLists)
                .HasForeignKey(d => d.CaseStatusId)
                .HasConstraintName("FK_ApplicationList_CaseStatusList");

            entity.HasOne(d => d.Rating).WithMany(p => p.ApplicationLists)
                .HasForeignKey(d => d.RatingId)
                .HasConstraintName("FK_ApplicationList_Rating");

            entity.HasOne(d => d.Resume).WithMany(p => p.ApplicationLists)
                .HasForeignKey(d => d.ResumeId)
                .HasConstraintName("FK_ApplicationList_Resume");
        });

        modelBuilder.Entity<CaseSkill>(entity =>
        {
            entity.ToTable("CaseSkill");

            entity.Property(e => e.CaseSkillId).HasColumnName("CaseSkillID");
            entity.Property(e => e.CaseId).HasColumnName("CaseID");
            entity.Property(e => e.SkillId).HasColumnName("SkillID");

            entity.HasOne(d => d.Case).WithMany(p => p.CaseSkills)
                .HasForeignKey(d => d.CaseId)
                .HasConstraintName("FK_CaseSkill_TaskList");

            entity.HasOne(d => d.Skill).WithMany(p => p.CaseSkills)
                .HasForeignKey(d => d.SkillId)
                .HasConstraintName("FK_CaseSkill_Skill");
        });

        modelBuilder.Entity<CaseStatusList>(entity =>
        {
            entity.HasKey(e => e.CaseStatusId);

            entity.ToTable("CaseStatusList");

            entity.Property(e => e.CaseStatusId)
                .ValueGeneratedNever()
                .HasColumnName("CaseStatusID");
            entity.Property(e => e.CaseStatus).HasMaxLength(50);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK_TradeType");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(50);
        });

        modelBuilder.Entity<Certificate>(entity =>
        {
            entity.ToTable("Certificate");

            entity.Property(e => e.CertificateId).HasColumnName("CertificateID");
            entity.Property(e => e.CertificateName).HasMaxLength(50);
            entity.Property(e => e.CertificateTypeId).HasColumnName("CertificateTypeID");

            entity.HasOne(d => d.CertificateType).WithMany(p => p.Certificates)
                .HasForeignKey(d => d.CertificateTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Certificate_CertificateType");
        });

        modelBuilder.Entity<CertificateType>(entity =>
        {
            entity.ToTable("CertificateType");

            entity.Property(e => e.CertificateTypeId).HasColumnName("CertificateTypeID");
            entity.Property(e => e.CertificateTypeName).HasMaxLength(50);
        });

        modelBuilder.Entity<ChatBlockList>(entity =>
        {
            entity.ToTable("ChatBlockList");

            entity.Property(e => e.ChatBlockListId).HasColumnName("ChatBlockListID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.BlockedId).HasColumnName("BlockedID");

            entity.HasOne(d => d.Account).WithMany(p => p.ChatBlockListAccounts)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChatBlockList_MemberAccount");

            entity.HasOne(d => d.Blocked).WithMany(p => p.ChatBlockListBlockeds)
                .HasForeignKey(d => d.BlockedId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChatBlockList_MemberBlocked");
        });

        modelBuilder.Entity<ChatMessage>(entity =>
        {
            entity.ToTable("ChatMessage");

            entity.Property(e => e.ChatMessageId).HasColumnName("ChatMessageID");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ReceiverId).HasColumnName("ReceiverID");
            entity.Property(e => e.SenderId).HasColumnName("SenderID");

            entity.HasOne(d => d.Receiver).WithMany(p => p.ChatMessageReceivers)
                .HasForeignKey(d => d.ReceiverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChatMessage_MemberAccountReceiver");

            entity.HasOne(d => d.Sender).WithMany(p => p.ChatMessageSenders)
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChatMessage_MemberAccountSender");
        });

        modelBuilder.Entity<CheckIn>(entity =>
        {
            entity.HasKey(e => e.CheckId);

            entity.ToTable("CheckIn");

            entity.Property(e => e.CheckId).HasColumnName("CheckID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.CheckDate).HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.CheckIns)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CheckIn_MemberAccount");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.ToTable("City");

            entity.Property(e => e.CityId).HasColumnName("CityID");
            entity.Property(e => e.City1)
                .HasMaxLength(50)
                .HasColumnName("City");
        });

        modelBuilder.Entity<ExpertApplication>(entity =>
        {
            entity.HasKey(e => e.ApplicationListId);

            entity.ToTable("ExpertApplication");

            entity.Property(e => e.ApplicationListId).HasColumnName("ApplicationListID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.CaseId).HasColumnName("CaseID");
            entity.Property(e => e.CaseStatusId).HasColumnName("CaseStatusID");
            entity.Property(e => e.ExpertAccountId).HasColumnName("ExpertAccountID");
            entity.Property(e => e.RatingId).HasColumnName("RatingID");

            entity.HasOne(d => d.Account).WithMany(p => p.ExpertApplications)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_ExpertApplication_MemberAccount");

            entity.HasOne(d => d.Case).WithMany(p => p.ExpertApplications)
                .HasForeignKey(d => d.CaseId)
                .HasConstraintName("FK_ExpertApplication_TaskList");

            entity.HasOne(d => d.CaseStatus).WithMany(p => p.ExpertApplications)
                .HasForeignKey(d => d.CaseStatusId)
                .HasConstraintName("FK_ExpertApplication_CaseStatusList");

            entity.HasOne(d => d.Rating).WithMany(p => p.ExpertApplications)
                .HasForeignKey(d => d.RatingId)
                .HasConstraintName("FK_ExpertApplication_Rating");
        });

        modelBuilder.Entity<ExpertResume>(entity =>
        {
            entity.HasKey(e => e.ResumeId);

            entity.ToTable("ExpertResume");

            entity.Property(e => e.ResumeId)
                .ValueGeneratedNever()
                .HasColumnName("ResumeID");
            entity.Property(e => e.CommonPrice).HasColumnType("money");
            entity.Property(e => e.ContactMethod).HasMaxLength(50);
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.ServiceMethod).HasMaxLength(50);
            entity.Property(e => e.WorksUrl).HasMaxLength(50);

            entity.HasOne(d => d.Resume).WithOne(p => p.ExpertResume)
                .HasForeignKey<ExpertResume>(d => d.ResumeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ExpertResume_Resume");
        });

        modelBuilder.Entity<ExpertWork>(entity =>
        {
            entity.HasKey(e => e.WorksId);

            entity.Property(e => e.WorksId).HasColumnName("WorksID");
            entity.Property(e => e.DataCreateDate).HasColumnType("datetime");
            entity.Property(e => e.Workname).HasMaxLength(50);
        });

        modelBuilder.Entity<ExpertWorkList>(entity =>
        {
            entity.HasKey(e => e.WorksListId);

            entity.ToTable("ExpertWorkList");

            entity.Property(e => e.WorksListId).HasColumnName("WorksListID");
            entity.Property(e => e.ResumeId).HasColumnName("ResumeID");
            entity.Property(e => e.WorksId).HasColumnName("WorksID");

            entity.HasOne(d => d.Resume).WithMany(p => p.ExpertWorkLists)
                .HasForeignKey(d => d.ResumeId)
                .HasConstraintName("FK_ExpertWorkList_ExpertResume");

            entity.HasOne(d => d.Works).WithMany(p => p.ExpertWorkLists)
                .HasForeignKey(d => d.WorksId)
                .HasConstraintName("FK_ExpertWorkList_ExpertWorks");
        });

        modelBuilder.Entity<ForgetPassword>(entity =>
        {
            entity.HasKey(e => e.ForgetId);

            entity.ToTable("ForgetPassword");

            entity.Property(e => e.ForgetId).HasColumnName("ForgetID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.CreateTime).HasColumnType("datetime");
            entity.Property(e => e.ForgetCountInAweek).HasColumnName("ForgetCountInAWeek");
            entity.Property(e => e.TemporaryPassword).HasMaxLength(10);

            entity.HasOne(d => d.Account).WithMany(p => p.ForgetPasswords)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ForgetPassword_MemberAccount");
        });

        modelBuilder.Entity<ForumCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK_forumCategory");

            entity.ToTable("ForumCategory");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(100);
        });

        modelBuilder.Entity<ForumPost>(entity =>
        {
            entity.HasKey(e => e.PostId).HasName("PK_forumPost");

            entity.ToTable("ForumPost");

            entity.Property(e => e.PostId).HasColumnName("PostID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LikeCount).HasDefaultValueSql("((0))");
            entity.Property(e => e.ParentId).HasColumnName("ParentID");
            entity.Property(e => e.Status).HasDefaultValueSql("((1))");
            entity.Property(e => e.Title).HasMaxLength(100);
            entity.Property(e => e.Updated).HasColumnType("datetime");
            entity.Property(e => e.ViewCount).HasDefaultValueSql("((0))");

            entity.HasOne(d => d.Account).WithMany(p => p.ForumPosts)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ForumPost_MemberAccount");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK_forumPost_forumPost");

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.ForumPosts)
                .HasForeignKey(d => d.Status)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ForumPost_ForumStatus");
        });

        modelBuilder.Entity<ForumPostCategory>(entity =>
        {
            entity.HasKey(e => e.PostCategoryId);

            entity.ToTable("ForumPostCategory");

            entity.Property(e => e.PostCategoryId).HasColumnName("PostCategoryID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.PostId).HasColumnName("PostID");

            entity.HasOne(d => d.Category).WithMany(p => p.ForumPostCategories)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ForumPostCategory_ForumCategory");

            entity.HasOne(d => d.Post).WithMany(p => p.ForumPostCategories)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ForumPostCategory_forumPost");
        });

        modelBuilder.Entity<ForumPostComment>(entity =>
        {
            entity.HasKey(e => e.PostCommentId).HasName("PK_forumPostComment");

            entity.ToTable("ForumPostComment");

            entity.Property(e => e.PostCommentId).HasColumnName("PostCommentID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.Comment).HasColumnType("text");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ParentId).HasColumnName("ParentID");
            entity.Property(e => e.PostId).HasColumnName("PostID");
            entity.Property(e => e.Status).HasDefaultValueSql("((1))");
            entity.Property(e => e.Updated).HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.ForumPostComments)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ForumPostComment_MemberAccount");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK_ForumPostComment_ForumPostComment");

            entity.HasOne(d => d.Post).WithMany(p => p.ForumPostComments)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ForumPostComment_forumPost");

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.ForumPostComments)
                .HasForeignKey(d => d.Status)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ForumPostComment_ForumStatus");
        });

        modelBuilder.Entity<ForumPostLike>(entity =>
        {
            entity.HasKey(e => e.PostLikeId);

            entity.ToTable("ForumPostLike");

            entity.Property(e => e.PostLikeId).HasColumnName("PostLikeID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PostId).HasColumnName("PostID");

            entity.HasOne(d => d.Post).WithMany(p => p.ForumPostLikes)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ForumPostLike_ForumPost");
        });

        modelBuilder.Entity<ForumStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId);

            entity.ToTable("ForumStatus");

            entity.Property(e => e.StatusId)
                .ValueGeneratedOnAdd()
                .HasColumnName("StatusID");
            entity.Property(e => e.StatusName).HasMaxLength(50);
        });

        modelBuilder.Entity<HumanList>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("HumanList");

            entity.Property(e => e.ApplicantId).HasColumnName("ApplicantID");
            entity.Property(e => e.CaseId).HasColumnName("CaseID");
            entity.Property(e => e.CaseStatusId).HasColumnName("CaseStatusID");
            entity.Property(e => e.HumanListId)
                .ValueGeneratedOnAdd()
                .HasColumnName("HumanListID");
            entity.Property(e => e.SenderId).HasColumnName("SenderID");
        });

        modelBuilder.Entity<Keyword>(entity =>
        {
            entity.ToTable("Keyword");

            entity.Property(e => e.KeywordId).HasColumnName("KeywordID");
            entity.Property(e => e.KeywordContent).HasMaxLength(50);
        });

        modelBuilder.Entity<LoginHistory>(entity =>
        {
            entity.HasKey(e => e.LoginId);

            entity.ToTable("LoginHistory");

            entity.Property(e => e.LoginId).HasColumnName("LoginID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.LoginSF).HasColumnName("LoginS/F");
            entity.Property(e => e.LoginTime).HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.LoginHistories)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LoginHistory_MemberAccount");
        });

        modelBuilder.Entity<MemberAccount>(entity =>
        {
            entity.HasKey(e => e.AccountId);

            entity.ToTable("MemberAccount");

            entity.HasIndex(e => e.Email, "IX_MemberAccount").IsUnique();

            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.AccountStatus)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.BitrhDay).HasColumnType("datetime");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Gender).HasMaxLength(2);
            entity.Property(e => e.IdcardNo)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("IDCardNo");
            entity.Property(e => e.Name).HasMaxLength(10);
            entity.Property(e => e.Password).HasMaxLength(10);
            entity.Property(e => e.PhoneNo)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.UserName).HasMaxLength(10);
        });

        modelBuilder.Entity<MemberCollection>(entity =>
        {
            entity.HasKey(e => e.CollectionId);

            entity.ToTable("MemberCollection");

            entity.Property(e => e.CollectionId).HasColumnName("CollectionID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.CaseId).HasColumnName("CaseID");
            entity.Property(e => e.ResumeId).HasColumnName("ResumeID");

            entity.HasOne(d => d.Account).WithMany(p => p.MemberCollections)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MemberCollection_MemberAccount");

            entity.HasOne(d => d.Case).WithMany(p => p.MemberCollections)
                .HasForeignKey(d => d.CaseId)
                .HasConstraintName("FK_MemberCollection_TaskList");

            entity.HasOne(d => d.Resume).WithMany(p => p.MemberCollections)
                .HasForeignKey(d => d.ResumeId)
                .HasConstraintName("FK_MemberCollection_Resume");
        });

        modelBuilder.Entity<MemberRoleConn>(entity =>
        {
            entity.HasKey(e => e.MemberRoleId);

            entity.ToTable("MemberRoleConn");

            entity.Property(e => e.MemberRoleId).HasColumnName("MemberRoleID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.RoleId).HasColumnName("RoleID");

            entity.HasOne(d => d.Account).WithMany(p => p.MemberRoleConns)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MemberRoleConn_MemberAccount");

            entity.HasOne(d => d.Role).WithMany(p => p.MemberRoleConns)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MemberRoleConn_RoleInfo");
        });

        modelBuilder.Entity<MemberStatusList>(entity =>
        {
            entity.HasKey(e => e.StatusChangeId);

            entity.ToTable("MemberStatusList");

            entity.Property(e => e.StatusChangeId).HasColumnName("StatusChangeID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.StatusChangeReasonId).HasColumnName("StatusChangeReasonID");
            entity.Property(e => e.UpdateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.StatusChangeReason).WithMany(p => p.MemberStatusLists)
                .HasForeignKey(d => d.StatusChangeReasonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MemberStatusList_StatusChangeReason");

            entity.HasOne(d => d.UpdateUserNavigation).WithMany(p => p.MemberStatusLists)
                .HasForeignKey(d => d.UpdateUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MemberStatusList_MemberAccount");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Order");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CreateTime).HasColumnType("datetime");
            entity.Property(e => e.PayWayId).HasColumnName("PayWayID");
            entity.Property(e => e.StatusId).HasColumnName("StatusID");

            entity.HasOne(d => d.Account).WithMany(p => p.Orders)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_Order_MemberAccount");

            entity.HasOne(d => d.Category).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Order_TradeType");

            entity.HasOne(d => d.PayWay).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PayWayId)
                .HasConstraintName("FK_Order_PayWay");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.ToTable("OrderDetail");

            entity.Property(e => e.OrderdetailId).HasColumnName("OrderdetailID");
            entity.Property(e => e.CaseId).HasColumnName("CaseID");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.ResumeId).HasColumnName("ResumeID");
            entity.Property(e => e.TopType).HasMaxLength(50);

            entity.HasOne(d => d.Case).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.CaseId)
                .HasConstraintName("FK_OrderDetail_TaskList");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_OrderDetail_Order");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_OrderDetail_Product");

            entity.HasOne(d => d.Resume).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ResumeId)
                .HasConstraintName("FK_OrderDetail_Resume");
        });

        modelBuilder.Entity<PayWay>(entity =>
        {
            entity.ToTable("PayWay");

            entity.Property(e => e.PayWayId).HasColumnName("PayWayID");
            entity.Property(e => e.PayWayName).HasMaxLength(50);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.ToTable("Payment");

            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.Payment1)
                .HasMaxLength(50)
                .HasColumnName("Payment");
        });

        modelBuilder.Entity<PaymentDate>(entity =>
        {
            entity.ToTable("PaymentDate");

            entity.Property(e => e.PaymentDateId).HasColumnName("PaymentDateID");
            entity.Property(e => e.PaymentDate1)
                .HasMaxLength(50)
                .HasColumnName("PaymentDate");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CoverPhoto).HasMaxLength(50);
            entity.Property(e => e.PostEndDate).HasColumnType("datetime");
            entity.Property(e => e.PostStartDate).HasColumnType("datetime");
            entity.Property(e => e.ProductName).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TopType).HasMaxLength(50);

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Product_Category");
        });

        modelBuilder.Entity<ProductPhoto>(entity =>
        {
            entity.ToTable("ProductPhoto");

            entity.Property(e => e.ProductPhotoId).HasColumnName("ProductPhotoID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.ProductPhoto1)
                .HasMaxLength(50)
                .HasColumnName("ProductPhoto");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductPhotos)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_ProductPhoto_Product");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.ToTable("Rating");

            entity.Property(e => e.RatingId).HasColumnName("RatingID");
            entity.Property(e => e.RatingContent).HasMaxLength(50);
            entity.Property(e => e.RatingDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.RatingStar).HasMaxLength(50);
            entity.Property(e => e.SourceAccountId).HasColumnName("SourceAccountID");
            entity.Property(e => e.SourceRoleId).HasColumnName("SourceRoleID");
            entity.Property(e => e.TargetAccountId).HasColumnName("TargetAccountID");
            entity.Property(e => e.TargetRoleId).HasColumnName("TargetRoleID");
        });

        modelBuilder.Entity<Resume>(entity =>
        {
            entity.ToTable("Resume");

            entity.Property(e => e.ResumeId).HasColumnName("ResumeID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.Address).HasMaxLength(50);
            entity.Property(e => e.Autobiography).HasMaxLength(400);
            entity.Property(e => e.CaseStatusId).HasColumnName("CaseStatusID");
            entity.Property(e => e.TaskNameId).HasColumnName("TaskNameID");
            entity.Property(e => e.TownId).HasColumnName("TownID");
            entity.Property(e => e.WorkingHoursId).HasColumnName("WorkingHoursID");

            entity.HasOne(d => d.Account).WithMany(p => p.Resumes)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Resume_MemberAccount");

            entity.HasOne(d => d.CaseStatus).WithMany(p => p.Resumes)
                .HasForeignKey(d => d.CaseStatusId)
                .HasConstraintName("FK_Resume_CaseStatusList");

            entity.HasOne(d => d.TaskName).WithMany(p => p.Resumes)
                .HasForeignKey(d => d.TaskNameId)
                .HasConstraintName("FK_Resume_TaskNameList");

            entity.HasOne(d => d.Town).WithMany(p => p.Resumes)
                .HasForeignKey(d => d.TownId)
                .HasConstraintName("FK_Resume_Town");

            entity.HasOne(d => d.WorkingHours).WithMany(p => p.Resumes)
                .HasForeignKey(d => d.WorkingHoursId)
                .HasConstraintName("FK_Resume_WorkingTime");
        });

        modelBuilder.Entity<ResumeCertificate>(entity =>
        {
            entity.ToTable("ResumeCertificate");

            entity.Property(e => e.ResumeCertificateId).HasColumnName("ResumeCertificateID");
            entity.Property(e => e.CertificateId).HasColumnName("CertificateID");
            entity.Property(e => e.ResumeId).HasColumnName("ResumeID");

            entity.HasOne(d => d.Certificate).WithMany(p => p.ResumeCertificates)
                .HasForeignKey(d => d.CertificateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ResumeCertificate_Certificate");

            entity.HasOne(d => d.Resume).WithMany(p => p.ResumeCertificates)
                .HasForeignKey(d => d.ResumeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ResumeCertificate_Resume1");
        });

        modelBuilder.Entity<ResumeKeywordList>(entity =>
        {
            entity.ToTable("ResumeKeywordList");

            entity.Property(e => e.ResumeKeywordListId).HasColumnName("ResumeKeywordListID");
            entity.Property(e => e.KeywordId).HasColumnName("KeywordID");
            entity.Property(e => e.ResumeId).HasColumnName("ResumeID");

            entity.HasOne(d => d.Keyword).WithMany(p => p.ResumeKeywordLists)
                .HasForeignKey(d => d.KeywordId)
                .HasConstraintName("FK_ResumeKeywordList_Keyword");

            entity.HasOne(d => d.Resume).WithMany(p => p.ResumeKeywordLists)
                .HasForeignKey(d => d.ResumeId)
                .HasConstraintName("FK_ResumeKeywordList_Resume");
        });

        modelBuilder.Entity<ResumeSkill>(entity =>
        {
            entity.ToTable("ResumeSkill");

            entity.Property(e => e.ResumeSkillId).HasColumnName("ResumeSkillID");
            entity.Property(e => e.ResumeId).HasColumnName("ResumeID");
            entity.Property(e => e.SkillId).HasColumnName("SkillID");

            entity.HasOne(d => d.Resume).WithMany(p => p.ResumeSkills)
                .HasForeignKey(d => d.ResumeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ResumeSkill_Resume");

            entity.HasOne(d => d.Skill).WithMany(p => p.ResumeSkills)
                .HasForeignKey(d => d.SkillId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ResumeSkill_Skill");
        });

        modelBuilder.Entity<RoleInfo>(entity =>
        {
            entity.HasKey(e => e.RoleId);

            entity.ToTable("RoleInfo");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.RoleName).HasMaxLength(10);
        });

        modelBuilder.Entity<Salary>(entity =>
        {
            entity.ToTable("Salary");

            entity.Property(e => e.SalaryId).HasColumnName("SalaryID");
            entity.Property(e => e.PaymentDateId).HasColumnName("PaymentDateID");
            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
        });

        modelBuilder.Entity<ServiceContact>(entity =>
        {
            entity.ToTable("ServiceContact");

            entity.Property(e => e.ServiceContactId).HasColumnName("ServiceContactID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.ComplaintTitle).HasMaxLength(50);
            entity.Property(e => e.ProcessStatus)
                .IsRequired()
                .HasDefaultValueSql("((1))");

            entity.HasOne(d => d.Account).WithMany(p => p.ServiceContacts)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ServiceContact_MemberAccount");
        });

        modelBuilder.Entity<Skill>(entity =>
        {
            entity.ToTable("Skill");

            entity.Property(e => e.SkillId).HasColumnName("SkillID");
            entity.Property(e => e.SkillName).HasMaxLength(50);
            entity.Property(e => e.SkillTypeId).HasColumnName("SkillTypeID");

            entity.HasOne(d => d.SkillType).WithMany(p => p.Skills)
                .HasForeignKey(d => d.SkillTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Skill_SkillType");
        });

        modelBuilder.Entity<SkillType>(entity =>
        {
            entity.ToTable("SkillType");

            entity.Property(e => e.SkillTypeId).HasColumnName("SkillTypeID");
            entity.Property(e => e.SkillTypeName).HasMaxLength(50);
        });

        modelBuilder.Entity<StatusChangeReasonList>(entity =>
        {
            entity.HasKey(e => e.StatusChangeReasonId).HasName("PK_StatusChangeReason");

            entity.ToTable("StatusChangeReasonList");

            entity.Property(e => e.StatusChangeReasonId)
                .ValueGeneratedNever()
                .HasColumnName("StatusChangeReasonID");
            entity.Property(e => e.StatusChangeReason).HasMaxLength(50);
        });

        modelBuilder.Entity<TaskCertificate>(entity =>
        {
            entity.ToTable("TaskCertificate");

            entity.Property(e => e.TaskCertificateId).HasColumnName("TaskCertificateID");
            entity.Property(e => e.CaseId).HasColumnName("CaseID");
            entity.Property(e => e.CertficateId).HasColumnName("CertficateID");

            entity.HasOne(d => d.Case).WithMany(p => p.TaskCertificates)
                .HasForeignKey(d => d.CaseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskCertificate_TaskList");
        });

        modelBuilder.Entity<TaskKeywordList>(entity =>
        {
            entity.ToTable("TaskKeywordList");

            entity.Property(e => e.TaskKeywordListId).HasColumnName("TaskKeywordListID");
            entity.Property(e => e.CaseId).HasColumnName("CaseID");
            entity.Property(e => e.KeywordId).HasColumnName("KeywordID");

            entity.HasOne(d => d.Case).WithMany(p => p.TaskKeywordLists)
                .HasForeignKey(d => d.CaseId)
                .HasConstraintName("FK_TaskKeywordList_TaskList");

            entity.HasOne(d => d.Keyword).WithMany(p => p.TaskKeywordLists)
                .HasForeignKey(d => d.KeywordId)
                .HasConstraintName("FK_TaskKeywordList_Keyword");
        });

        modelBuilder.Entity<TaskList>(entity =>
        {
            entity.HasKey(e => e.CaseId);

            entity.ToTable("TaskList");

            entity.Property(e => e.CaseId).HasColumnName("CaseID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.Address).HasMaxLength(50);
            entity.Property(e => e.CaseStatusId).HasColumnName("CaseStatusID");
            entity.Property(e => e.DataCreateDate).HasMaxLength(50);
            entity.Property(e => e.DataModifyDate).HasMaxLength(50);
            entity.Property(e => e.DataModifyPerson).HasMaxLength(50);
            entity.Property(e => e.HumanList).HasMaxLength(50);
            entity.Property(e => e.LanguageRequired).HasMaxLength(50);
            entity.Property(e => e.PaymentDateId).HasColumnName("PaymentDateID");
            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.PublishEnd).HasMaxLength(50);
            entity.Property(e => e.PublishOrNot).HasMaxLength(50);
            entity.Property(e => e.PublishStart).HasMaxLength(50);
            entity.Property(e => e.RequiredNum).HasMaxLength(50);
            entity.Property(e => e.Requirement).HasMaxLength(50);
            entity.Property(e => e.SalaryId).HasColumnName("SalaryID");
            entity.Property(e => e.StatusChangeReasonId).HasColumnName("StatusChangeReasonID");
            entity.Property(e => e.TaskEndDate).HasMaxLength(50);
            entity.Property(e => e.TaskEndHour).HasMaxLength(50);
            entity.Property(e => e.TaskNameId).HasColumnName("TaskNameID");
            entity.Property(e => e.TaskPeriod).HasMaxLength(50);
            entity.Property(e => e.TaskStartDate).HasMaxLength(50);
            entity.Property(e => e.TaskStartHour).HasMaxLength(50);
            entity.Property(e => e.TaskTitle).HasMaxLength(50);
            entity.Property(e => e.TownId).HasColumnName("TownID");
            entity.Property(e => e.WorkingHoursId).HasColumnName("WorkingHoursID");

            entity.HasOne(d => d.PaymentDate).WithMany(p => p.TaskLists)
                .HasForeignKey(d => d.PaymentDateId)
                .HasConstraintName("FK_TaskList_PaymentDate");

            entity.HasOne(d => d.Payment).WithMany(p => p.TaskLists)
                .HasForeignKey(d => d.PaymentId)
                .HasConstraintName("FK_TaskList_Payment");

            entity.HasOne(d => d.Salary).WithMany(p => p.TaskLists)
                .HasForeignKey(d => d.SalaryId)
                .HasConstraintName("FK_TaskList_Salary");

            entity.HasOne(d => d.TaskName).WithMany(p => p.TaskLists)
                .HasForeignKey(d => d.TaskNameId)
                .HasConstraintName("FK_TaskList_TaskNameList");

            entity.HasOne(d => d.Town).WithMany(p => p.TaskLists)
                .HasForeignKey(d => d.TownId)
                .HasConstraintName("FK_TaskList_Town");
        });

        modelBuilder.Entity<TaskNameList>(entity =>
        {
            entity.HasKey(e => e.TaskNameId).HasName("PK_TaskName");

            entity.ToTable("TaskNameList");

            entity.Property(e => e.TaskNameId).HasColumnName("TaskNameID");
            entity.Property(e => e.TaskName).HasMaxLength(50);
        });

        modelBuilder.Entity<TaskPhoto>(entity =>
        {
            entity.HasKey(e => e.TaskPhotoId).HasName("PK_TaskPhoto_1");

            entity.ToTable("TaskPhoto");

            entity.Property(e => e.TaskPhotoId).HasColumnName("TaskPhotoID");
            entity.Property(e => e.CaseId).HasColumnName("CaseID");

            entity.HasOne(d => d.Case).WithMany(p => p.TaskPhotos)
                .HasForeignKey(d => d.CaseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskPhoto_TaskList");
        });

        modelBuilder.Entity<TaskSkill>(entity =>
        {
            entity.ToTable("TaskSkill");

            entity.Property(e => e.TaskSkillId).HasColumnName("TaskSkillID");
            entity.Property(e => e.CaseId).HasColumnName("CaseID");
            entity.Property(e => e.SkillId).HasColumnName("SkillID");

            entity.HasOne(d => d.Case).WithMany(p => p.TaskSkills)
                .HasForeignKey(d => d.CaseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskSkill_TaskList");
        });

        modelBuilder.Entity<Town>(entity =>
        {
            entity.ToTable("Town");

            entity.Property(e => e.TownId).HasColumnName("TownID");
            entity.Property(e => e.CityId).HasColumnName("CityID");
            entity.Property(e => e.Town1)
                .HasMaxLength(10)
                .HasColumnName("Town");

            entity.HasOne(d => d.City).WithMany(p => p.Towns)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Town_City");
        });

        modelBuilder.Entity<WorkingTime>(entity =>
        {
            entity.HasKey(e => e.WorkingHoursId);

            entity.ToTable("WorkingTime");

            entity.Property(e => e.WorkingHoursId).HasColumnName("WorkingHoursID");
            entity.Property(e => e.WorkingDateFrom).HasMaxLength(50);
            entity.Property(e => e.WorkingDateTo).HasMaxLength(50);
            entity.Property(e => e.WorkingHoursFrom).HasMaxLength(50);
            entity.Property(e => e.WorkingHoursTo).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
