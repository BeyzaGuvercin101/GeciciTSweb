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

    public virtual DbSet<RequestLog> RequestLogs { get; set; }

    public virtual DbSet<IntegrityRiskAssessment> IntegrityRiskAssessments { get; set; }

    public virtual DbSet<MaintenanceRiskAssessment> MaintenanceRiskAssessments { get; set; }

    public virtual DbSet<ProductionRiskAssessment> ProductionRiskAssessments { get; set; }

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
                .OnDelete(DeleteBehavior.ClientSetNull);
        });



        modelBuilder.Entity<MaintenanceRequest>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.UnitId);
            entity.HasIndex(e => e.CreatedByUserId);
            
            entity.Property(e => e.BildirimNumarasi).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.EquipmentNumber).HasMaxLength(100);
            entity.Property(e => e.Fluid).HasMaxLength(100);
            entity.Property(e => e.Pressure).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Temperature).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.MaintenanceRequests)
                .HasForeignKey(d => d.CreatedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.TempMaintenanceType).WithMany(p => p.MaintenanceRequests)
                .HasForeignKey(d => d.TempMaintenanceTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Unit).WithMany(p => p.MaintenanceRequests)
                .HasForeignKey(d => d.UnitId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<RequestLog>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.MaintenanceRequestId);
            entity.HasIndex(e => e.AuthorUserId);
            
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ActionType).HasMaxLength(50);
            entity.Property(e => e.ActionNote).HasMaxLength(1000);

            entity.HasOne(d => d.AuthorUser).WithMany(p => p.RequestLogs)
                .HasForeignKey(d => d.AuthorUserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.MaintenanceRequest).WithMany(p => p.RequestLogs)
                .HasForeignKey(d => d.MaintenanceRequestId)
                .OnDelete(DeleteBehavior.ClientSetNull);
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

        // Risk Assessment Entity Configurations
        modelBuilder.Entity<IntegrityRiskAssessment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.MaintenanceRequestId);
            entity.HasIndex(e => e.UserId);
            
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.RiskNote).HasMaxLength(1000);
            entity.Property(e => e.RPNBefore).HasComputedColumnSql("[ImpactBefore] * [ProbabilityBefore]");
            entity.Property(e => e.RPNAfter).HasComputedColumnSql("[ImpactAfter] * [ProbabilityAfter]");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.MaintenanceRequest).WithMany(p => p.IntegrityRiskAssessments)
                .HasForeignKey(d => d.MaintenanceRequestId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<MaintenanceRiskAssessment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.MaintenanceRequestId);
            entity.HasIndex(e => e.UserId);
            
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.RiskNote).HasMaxLength(1000);
            entity.Property(e => e.RPNBefore).HasComputedColumnSql("[ImpactBefore] * [ProbabilityBefore]");
            entity.Property(e => e.RPNAfter).HasComputedColumnSql("[ImpactAfter] * [ProbabilityAfter]");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.MaintenanceRequest).WithMany(p => p.MaintenanceRiskAssessments)
                .HasForeignKey(d => d.MaintenanceRequestId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<ProductionRiskAssessment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.MaintenanceRequestId);
            entity.HasIndex(e => e.UserId);
            
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.RiskNote).HasMaxLength(1000);
            entity.Property(e => e.RPNBefore).HasComputedColumnSql("[ImpactBefore] * [ProbabilityBefore]");
            entity.Property(e => e.RPNAfter).HasComputedColumnSql("[ImpactAfter] * [ProbabilityAfter]");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.MaintenanceRequest).WithMany(p => p.ProductionRiskAssessments)
                .HasForeignKey(d => d.MaintenanceRequestId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        // User Entity Configuration (updated for Keycloak)
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.KeycloakSub).IsUnique();
            entity.Property(e => e.KeycloakSub).HasMaxLength(100);
        });

        // Soft Delete Query Filters
        modelBuilder.Entity<Companies>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Console>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<MaintenanceRequest>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<RequestLog>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<TemporaryMaintenanceType>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Unit>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<IntegrityRiskAssessment>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<MaintenanceRiskAssessment>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<ProductionRiskAssessment>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<User>().HasQueryFilter(e => !e.IsDeleted);


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

}
