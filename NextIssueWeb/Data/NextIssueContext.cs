using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NextIssueWeb.Models;

namespace NextIssueWeb.Data;

public partial class NextIssueContext : DbContext
{
    public NextIssueContext()
    {
    }

    public NextIssueContext(DbContextOptions<NextIssueContext> options)
        : base(options)
    {
    }

    public virtual DbSet<MergeissuePicture> MergeissuePictures { get; set; }

    public virtual DbSet<MergeprojectIssue> MergeprojectIssues { get; set; }

    public virtual DbSet<MergeuserPosition> MergeuserPositions { get; set; }

    public virtual DbSet<Ncomment> Ncomments { get; set; }

    public virtual DbSet<Nimportant> Nimportants { get; set; }

    public virtual DbSet<Nissue> Nissues { get; set; }

    public virtual DbSet<Nlogger> Nloggers { get; set; }

    public virtual DbSet<Npicture> Npictures { get; set; }

    public virtual DbSet<Nposition> Npositions { get; set; }

    public virtual DbSet<Nproject> Nprojects { get; set; }

    public virtual DbSet<Nstatus> Nstatuses { get; set; }

    public virtual DbSet<Nuser> Nusers { get; set; }

    public virtual DbSet<SystemCompany> SystemCompanies { get; set; }

    public virtual DbSet<SystemLicense> SystemLicenses { get; set; }

    public virtual DbSet<SystemOnDate> SystemOnDates { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-3C87OGA;Initial Catalog=NextIssue;Integrated Security=True;Encrypt=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MergeissuePicture>(entity =>
        {
            entity.ToTable("MERGEissue_picture");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IssueId).HasColumnName("issue_id");
            entity.Property(e => e.PictureId).HasColumnName("picture_id");
        });

        modelBuilder.Entity<MergeprojectIssue>(entity =>
        {
            entity.ToTable("MERGEproject_issue");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IssueId).HasColumnName("Issue_id");
            entity.Property(e => e.ProjectId).HasColumnName("Project_id");

            entity.HasOne(d => d.Issue).WithMany(p => p.MergeprojectIssues)
                .HasForeignKey(d => d.IssueId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MERGEproject_issue_Nissue");

            entity.HasOne(d => d.Project).WithMany(p => p.MergeprojectIssues)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MERGEproject_issue_NProject");
        });

        modelBuilder.Entity<MergeuserPosition>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("MERGEuser_position");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.PositionId).HasColumnName("Position_id");
            entity.Property(e => e.UserId).HasColumnName("User_id");

            entity.HasOne(d => d.Position).WithMany()
                .HasForeignKey(d => d.PositionId)
                .HasConstraintName("FK_MERGEuser_position_Nposition");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_MERGEuser_position_Nuser");
        });

        modelBuilder.Entity<Ncomment>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("NComment");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.IssueId).HasColumnName("Issue_id");
        });

        modelBuilder.Entity<Nimportant>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Nimporta__3214EC27805671B2");

            entity.ToTable("Nimportant");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Nissue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Nissue__3214EC27EA589DB5");

            entity.ToTable("Nissue");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.ImportantId).HasColumnName("Important_id");
            entity.Property(e => e.InformerId).HasColumnName("Informer_id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ResponsibleGroupId).HasColumnName("ResponsibleGroup_id");
            entity.Property(e => e.ResponsibleId).HasColumnName("Responsible_id");
            entity.Property(e => e.StatusId).HasColumnName("Status_id");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Important).WithMany(p => p.Nissues)
                .HasForeignKey(d => d.ImportantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Nissue_Nimportant");
        });

        modelBuilder.Entity<Nlogger>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__NLogger__3214EC27321BDA57");

            entity.ToTable("NLogger");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Controller)
                .HasMaxLength(555)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Detail)
                .HasMaxLength(555)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(555)
                .IsUnicode(false);
            entity.Property(e => e.SystemId).HasColumnName("System_id");

            entity.HasOne(d => d.System).WithMany(p => p.Nloggers)
                .HasForeignKey(d => d.SystemId)
                .HasConstraintName("FK_NLogger_SystemOnDate");
        });

        modelBuilder.Entity<Npicture>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Npicture__3214EC27788D98FA");

            entity.ToTable("Npicture");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Picture).HasColumnName("picture");
            entity.Property(e => e.UploadDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Nposition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Npositio__3214EC27CE3A7BD1");

            entity.ToTable("Nposition");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Nproject>(entity =>
        {
            entity.ToTable("NProject");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(500);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Nstatus>(entity =>
        {
            entity.ToTable("NStatus");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Nuser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Nuser__3214EC27045305FB");

            entity.ToTable("Nuser");

            entity.HasIndex(e => e.Username, "UQ__Nuser__536C85E4F1AB5A3F").IsUnique();

            entity.HasIndex(e => e.Aka, "UQ__Nuser__C6903796BF8C8E37").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Aka)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("AKA");
            entity.Property(e => e.CompanyId).HasColumnName("Company_id");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SystemCompany>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_SystemCompany_1");

            entity.ToTable("SystemCompany");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.LicenseId).HasColumnName("License_id");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.OwnerId).HasColumnName("Owner_id");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.Year).HasMaxLength(50);

            entity.HasOne(d => d.License).WithMany(p => p.SystemCompanies)
                .HasForeignKey(d => d.LicenseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SystemCompany_SystemLicense");

            entity.HasOne(d => d.Owner).WithMany(p => p.SystemCompanies)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SystemCompany_Nuser");
        });

        modelBuilder.Entity<SystemLicense>(entity =>
        {
            entity.ToTable("SystemLicense");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
        });

        modelBuilder.Entity<SystemOnDate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SystemOn__3214EC27A2303D0A");

            entity.ToTable("SystemOnDate");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.SystemDaydeleted).HasColumnName("SYSTEM_daydeleted");
            entity.Property(e => e.SystemNameEn)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SYSTEM_nameEN");
            entity.Property(e => e.SystemNameTh)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SYSTEM_nameTH");
            entity.Property(e => e.SystemStatus).HasColumnName("SYSTEM_status");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
