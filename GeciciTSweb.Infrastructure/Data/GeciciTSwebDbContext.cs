using System;
using System.Collections.Generic;
using GeciciTSweb.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Console = GeciciTSweb.Infrastructure.Entities.Console;

namespace GeciciTSweb.Infrastructure.Data;

public partial class GeciciTSwebDbContext : DbContext
{
    public GeciciTSwebDbContext(DbContextOptions<GeciciTSwebDbContext> options)
        : base(options)
    {

    }

    public virtual DbSet<Companies> Companies { get; set; }

    public virtual DbSet<Console> Consoles { get; set; }

    public virtual DbSet<MaintenanceRequest> MaintenanceRequests { get; set; }

    

    public virtual DbSet<RiskAssessment> RiskAssessments { get; set; }

    public virtual DbSet<TemporaryMaintenanceType> TemporaryMaintenanceTypes { get; set; }

    public virtual DbSet<Unit> Units { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Companies>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Name).IsUnique();
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Console>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.HasOne(d => d.Company).WithMany(p => p.Consoles)
                .HasForeignKey(d => d.CompanyId)
                 .OnDelete(DeleteBehavior.NoAction);
        });


        modelBuilder.Entity<TemporaryMaintenanceType>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Name).IsUnique();
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Unit>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.HasOne(d => d.Console).WithMany(p => p.Units)
                .HasForeignKey(d => d.ConsoleId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<RiskAssessment>()
                    .HasOne(r => r.User)
                    .WithMany(u => u.RiskAssessments)
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<RiskAssessment>()
                    .HasOne(r => r.MaintenanceRequest)
                    .WithMany(m => m.RiskAssessment)
                    .HasForeignKey(r => r.MaintenanceRequestId)
                    .OnDelete(DeleteBehavior.Restrict);


        // Soft Delete Query Filters
        modelBuilder.Entity<Companies>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Console>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<MaintenanceRequest>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<TemporaryMaintenanceType>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Unit>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<RiskAssessment>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<User>().HasQueryFilter(e => !e.IsDeleted);

        foreach (var relationship in modelBuilder.Model.GetEntityTypes()
        .SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.NoAction;
        }

        base.OnModelCreating(modelBuilder);

        //modelBuilder.Entity<MaintenanceRequest>(entity =>
        //{
        //    entity.HasKey(e => e.Id);
        //    entity.HasIndex(e => e.Status);
        //    entity.HasIndex(e => e.UnitId);
        //    entity.HasIndex(e => e.CreatedByUserId);
        //    entity.HasIndex(e => e.TempMaintenanceTypeId);

        //    entity.Property(e => e.NotificationNumber).HasMaxLength(50);
        //    entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
        //    entity.Property(e => e.EquipmentNumber).HasMaxLength(100);
        //    entity.Property(e => e.Fluid).HasMaxLength(100);
        //    entity.Property(e => e.Pressure).HasColumnType("decimal(10,2)");
        //    entity.Property(e => e.Status).HasConversion<string>().HasMaxLength(50);

        //    entity.Property(e => e.Temperature).HasColumnType("decimal(10,2)");


        //    entity.HasOne(d => d.TempMaintenanceType).WithMany(p => p.MaintenanceRequests)
        //        .HasForeignKey(d => d.TempMaintenanceTypeId)
        //        .OnDelete(DeleteBehavior.ClientSetNull);

        //    entity.HasOne(d => d.Unit).WithMany(p => p.MaintenanceRequests)
        //        .HasForeignKey(d => d.UnitId)
        //        .OnDelete(DeleteBehavior.ClientSetNull);
        //});




        //    // Risk Assessment Entity Configuration
        //    modelBuilder.Entity<RiskAssessment>(entity =>
        //    {
        //        entity.HasKey(e => e.Id);

        //        // Indexes
        //        entity.HasIndex(e => e.MaintenanceRequestId);
        //        entity.HasIndex(e => e.CreatedByUserId);
        //        entity.HasIndex(e => e.ApprovedByUserId);
        //        entity.HasIndex(e => e.DepartmentStatus).HasDatabaseName("IX_RA_Status");
        //        entity.HasIndex(e => e.CurrentRPN).HasDatabaseName("IX_RA_CurrentRPN");
        //        entity.HasIndex(e => e.ResidualRPN).HasDatabaseName("IX_RA_ResidualRPN");

        //        // Unique constraint for MaintenanceRequestId + DepartmentCode
        //        entity.HasIndex(e => new { e.MaintenanceRequestId, e.DepartmentCode })
        //              .IsUnique()
        //              .HasFilter("[IsDeleted] = 0")
        //              .HasDatabaseName("IX_RA_MaintenanceRequest_Department_Unique");

        //        // Property configurations
        //        entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
        //        entity.Property(e => e.DepartmentStatus).HasConversion<string>().HasMaxLength(30);
        //        entity.Property(e => e.ReturnReasonCode).HasMaxLength(500);
        //        entity.Property(e => e.ReturnReasonText).HasMaxLength(500);
        //        entity.Property(e => e.CancelReasonCode).HasMaxLength(500);
        //        entity.Property(e => e.CancelReasonText).HasMaxLength(500);
        //        entity.Property(e => e.RiskCategoryCode).HasMaxLength(50);
        //        entity.Property(e => e.DepartmentReportNote).HasMaxLength(500);
        //        entity.Property(e => e.OperationalRiskNote).HasMaxLength(500);
        //        entity.Property(e => e.PlannedTemporaryRepairDate).HasColumnType("DATETIME2(0)");
        //        entity.Property(e => e.PlannedPermanentRepairDate).HasColumnType("DATETIME2(0)");
        //        entity.Property(e => e.ApprovedAt).HasColumnType("DATETIME2(0)");
        //        entity.Property(e => e.CreatedAt).HasColumnType("DATETIME2(0)");
        //        entity.Property(e => e.UpdatedAt).HasColumnType("DATETIME2(0)");



        //        // Foreign key relationships
        //        entity.HasOne(d => d.CreatedByUser).WithMany(p => p.CreatedRiskAssessments)
        //            .HasForeignKey(d => d.CreatedByUserId)
        //            .OnDelete(DeleteBehavior.ClientSetNull);

        //        entity.HasOne(d => d.ApprovedByUser).WithMany(p => p.ApprovedRiskAssessments)
        //            .HasForeignKey(d => d.ApprovedByUserId)
        //            .OnDelete(DeleteBehavior.ClientSetNull);

        //        entity.HasOne(d => d.MaintenanceRequest).WithMany(p => p.RiskAssessments)
        //            .HasForeignKey(d => d.MaintenanceRequestId)
        //            .OnDelete(DeleteBehavior.ClientSetNull);
        //    });

        //    // User Entity Configuration
        //    modelBuilder.Entity<User>(entity =>
        //    {
        //        entity.HasKey(e => e.Id);
        //        entity.HasIndex(e => e.Username).IsUnique();
        //        entity.Property(e => e.Username).HasMaxLength(100);
        //    });




        //    OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

}
