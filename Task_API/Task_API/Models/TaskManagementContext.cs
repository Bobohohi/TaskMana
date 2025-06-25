using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Task_API.Models
{
    public partial class TaskManagementContext : DbContext
    {
        public TaskManagementContext()
        {
        }

        public TaskManagementContext(DbContextOptions<TaskManagementContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Board> Boards { get; set; } = null!;
        public virtual DbSet<Comment> Comments { get; set; } = null!;
        public virtual DbSet<Group> Groups { get; set; } = null!;
        public virtual DbSet<GroupDetail> GroupDetails { get; set; } = null!;
        public virtual DbSet<MembershipPlan> MembershipPlans { get; set; } = null!;
        public virtual DbSet<Notification> Notifications { get; set; } = null!;
        public virtual DbSet<Project> Projects { get; set; } = null!;
        public virtual DbSet<Report> Reports { get; set; } = null!;
        public virtual DbSet<Task> Tasks { get; set; } = null!;
        public virtual DbSet<TaskDetail> TaskDetails { get; set; } = null!;
        public virtual DbSet<TaskFile> TaskFiles { get; set; } = null!;
        public virtual DbSet<TaskHistory> TaskHistories { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserSub> UserSubs { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=LAPTOP-0UB387K3\\SQLEXPRESS;Database=TaskManagement;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Board>(entity =>
            {
                entity.ToTable("Board");

                entity.Property(e => e.BoardName).HasMaxLength(100);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Boards)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK__Board__ProjectId__4BAC3F29");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("Comment");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.TaskId)
                    .HasConstraintName("FK__Comment__TaskId__656C112C");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Comment__UserId__66603565");
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.ToTable("Group");

                entity.Property(e => e.GroupName).HasMaxLength(100);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Groups)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Group__UserId__403A8C7D");
            });

            modelBuilder.Entity<GroupDetail>(entity =>
            {
                entity.ToTable("Group_Detail");

                entity.Property(e => e.GroupDetailId).HasColumnName("Group_DetailId");

                entity.Property(e => e.RoleInGroup).HasMaxLength(50);

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.GroupDetails)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("FK__Group_Det__Group__4316F928");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.GroupDetails)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Group_Det__UserI__440B1D61");
            });

            modelBuilder.Entity<MembershipPlan>(entity =>
            {
                entity.HasKey(e => e.PlanId)
                    .HasName("PK__Membersh__755C22B7F74ED814");

                entity.ToTable("MembershipPlan");

                entity.Property(e => e.PlanName).HasMaxLength(100);

                entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.Property(e => e.IsRead).HasDefaultValueSql("((0))");

                entity.Property(e => e.Message).HasMaxLength(255);

                entity.Property(e => e.SentDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Notificat__UserI__6A30C649");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("Project");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.ProjectName).HasMaxLength(100);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Projects)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("FK__Project__GroupId__46E78A0C");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Projects)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Project__UserId__48CFD27E");
            });

            modelBuilder.Entity<Report>(entity =>
            {
                entity.ToTable("Report");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ReportName).HasMaxLength(100);

                entity.Property(e => e.ReportType).HasMaxLength(50);

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK__Report__ProjectI__6EF57B66");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Report__UserId__70DDC3D8");
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.ToTable("Task");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DueDate).HasColumnType("datetime");

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.HasOne(d => d.AssignedToNavigation)
                    .WithMany(p => p.TaskAssignedToNavigations)
                    .HasForeignKey(d => d.AssignedTo)
                    .HasConstraintName("FK__Task__AssignedTo__5070F446");

                entity.HasOne(d => d.Board)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.BoardId)
                    .HasConstraintName("FK__Task__BoardId__5441852A");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK__Task__ProjectId__5165187F");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TaskUsers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Task__UserId__534D60F1");
            });

            modelBuilder.Entity<TaskDetail>(entity =>
            {
                entity.ToTable("Task_Detail");

                entity.Property(e => e.TaskDetailId).HasColumnName("Task_DetailId");

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.TaskDetails)
                    .HasForeignKey(d => d.TaskId)
                    .HasConstraintName("FK__Task_Deta__TaskI__5812160E");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TaskDetails)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Task_Deta__UserI__571DF1D5");
            });

            modelBuilder.Entity<TaskFile>(entity =>
            {
                entity.HasKey(e => e.FileId)
                    .HasName("PK__TaskFile__6F0F98BFB8E2B7EA");

                entity.Property(e => e.FileName).HasMaxLength(255);

                entity.Property(e => e.FilePath).HasMaxLength(500);

                entity.Property(e => e.FileType).HasMaxLength(50);

                entity.Property(e => e.UploadedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.TaskFiles)
                    .HasForeignKey(d => d.TaskId)
                    .HasConstraintName("FK__TaskFiles__TaskI__5AEE82B9");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TaskFiles)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__TaskFiles__UserI__5CD6CB2B");
            });

            modelBuilder.Entity<TaskHistory>(entity =>
            {
                entity.HasKey(e => e.HistoryId)
                    .HasName("PK__TaskHist__4D7B4ABD3450944E");

                entity.ToTable("TaskHistory");

                entity.Property(e => e.Action).HasMaxLength(100);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.TaskHistories)
                    .HasForeignKey(d => d.TaskId)
                    .HasConstraintName("FK__TaskHisto__TaskI__5FB337D6");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.TaskHistories)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK__TaskHisto__Updat__60A75C0F");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Address).HasMaxLength(255);

                entity.Property(e => e.AuthProvider)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.GoogleId)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.PasswordHash)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PictureUrl)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Role)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Token)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserSub>(entity =>
            {
                entity.HasKey(e => e.SubscriptionId)
                    .HasName("PK__UserSubs__9A2B249D2DC80D72");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.Plan)
                    .WithMany(p => p.UserSubs)
                    .HasForeignKey(d => d.PlanId)
                    .HasConstraintName("FK__UserSubs__PlanId__3D5E1FD2");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserSubs)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__UserSubs__UserId__3C69FB99");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
