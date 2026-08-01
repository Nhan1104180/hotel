using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ServiceDBService.src.Domain.Entities;

namespace ServiceDBService.src.Infrastructure.Data;

public partial class ServiceDbContext : DbContext
{
    public ServiceDbContext()
    {
    }

    public ServiceDbContext(DbContextOptions<ServiceDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<ServiceCategory> ServiceCategories { get; set; }

    public virtual DbSet<ServiceUsage> ServiceUsages { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DBConnectionstring");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Services_3214EC07ADCB71C2");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasDefaultValue(1);

            entity.HasOne(d => d.Category).WithMany(p => p.Services).HasConstraintName("FK_Services_CategoryID__440B1D61");
        });

        modelBuilder.Entity<ServiceCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ServiceCategories_3214EC07ADCB71C2");
        });

        modelBuilder.Entity<ServiceUsage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ServiceUsages_3214EC07ADCB71C2");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Service).WithMany(p => p.ServiceUsages)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ServiceUsages_ServiceID__440B1D62");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
