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

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<MaintenanceRequest> MaintenanceRequests { get; set; }

    public virtual DbSet<RequestLog> RequestLogs { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

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

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Name).IsUnique();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<MaintenanceRequest>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.UnitId);
            
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
            
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.LogType).HasMaxLength(50);
            entity.Property(e => e.Reason).HasMaxLength(255);

            entity.HasOne(d => d.AuthorUser).WithMany(p => p.RequestLogs)
                .HasForeignKey(d => d.AuthorUserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.MaintenanceRequest).WithMany(p => p.RequestLogs)
                .HasForeignKey(d => d.MaintenanceRequestId);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Name).IsUnique();
            entity.Property(e => e.Name).HasMaxLength(50);
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
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.KeycloakUserId).IsUnique();
            entity.Property(e => e.KeycloakUserId).HasMaxLength(100);

            entity.HasOne(d => d.Department).WithMany(p => p.Users)
                .HasForeignKey(d => d.DepartmentId);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId);
        });
        modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);
        modelBuilder.Entity<Companies>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Console>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Department>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<MaintenanceRequest>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<RequestLog>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Role>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<TemporaryMaintenanceType>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Unit>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<User>().HasQueryFilter(e => !e.IsDeleted);


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

}
