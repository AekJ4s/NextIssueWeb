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

    public virtual DbSet<MergeuserPosition> MergeuserPositions { get; set; }

    public virtual DbSet<Nimportant> Nimportants { get; set; }

    public virtual DbSet<Nissue> Nissues { get; set; }

    public virtual DbSet<Nlogger> Nloggers { get; set; }

    public virtual DbSet<Npicture> Npictures { get; set; }

    public virtual DbSet<Nposition> Npositions { get; set; }

    public virtual DbSet<Nuser> Nusers { get; set; }

    public virtual DbSet<SystemOnDate> SystemOnDates { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-3C87OGA;Initial Catalog=NextIssue;Integrated Security=True;Encrypt=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MergeissuePicture>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Nissue_picture");

            entity.ToTable("MERGEissue_picture");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.IssueId).HasColumnName("issue_id");
            entity.Property(e => e.PictureId).HasColumnName("picture_id");
        });

        modelBuilder.Entity<MergeuserPosition>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("MERGEuser_position");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.PositionId)
                .HasMaxLength(50)
                .HasColumnName("Position_id");
            entity.Property(e => e.UserId).HasColumnName("User_id");

            entity.HasOne(d => d.Position).WithMany()
                .HasForeignKey(d => d.PositionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MERGEuser_position_Nposition");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MERGEuser_position_Nuser");
        });

        modelBuilder.Entity<Nimportant>(entity =>
        {
            entity.ToTable("Nimportant");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Nissue>(entity =>
        {
            entity.ToTable("Nissue");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.ImportantId).HasColumnName("Important_id");
            entity.Property(e => e.InformerId).HasColumnName("Informer_id");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.ResponsibleId).HasColumnName("Responsible_id");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Important).WithMany(p => p.Nissues)
                .HasForeignKey(d => d.ImportantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Nissue_Nimportant");
        });

        modelBuilder.Entity<Nlogger>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_NLogger_1");

            entity.ToTable("NLogger");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Controller).HasMaxLength(555);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Detail).HasMaxLength(555);
            entity.Property(e => e.Name).HasMaxLength(555);
            entity.Property(e => e.SystemId)
                .HasMaxLength(50)
                .HasColumnName("System_id");

            entity.HasOne(d => d.System).WithMany(p => p.Nloggers)
                .HasForeignKey(d => d.SystemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NLogger_SystemOnDate");
        });

        modelBuilder.Entity<Npicture>(entity =>
        {
            entity.ToTable("Npicture");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Picture).HasColumnName("picture");
            entity.Property(e => e.UploadDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Nposition>(entity =>
        {
            entity.ToTable("Nposition");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Nuser>(entity =>
        {
            entity.ToTable("Nuser");

            entity.HasIndex(e => e.Aka, "AKA").IsUnique();

            entity.HasIndex(e => e.Username, "IX_Nuser").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Aka)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("AKA");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<SystemOnDate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_NSystem");

            entity.ToTable("SystemOnDate");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.SystemDaydeleted).HasColumnName("SYSTEM_daydeleted");
            entity.Property(e => e.SystemNameEn)
                .HasMaxLength(50)
                .HasColumnName("SYSTEM_nameEN");
            entity.Property(e => e.SystemNameTh)
                .HasMaxLength(50)
                .HasColumnName("SYSTEM_nameTH");
            entity.Property(e => e.SystemStatus).HasColumnName("SYSTEM_status");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
