using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PaymentDBService.src.Domain.Entities;

namespace PaymentDBService.src.Infrastructure.Data;

public partial class PaymentDbContext : DbContext
{
    public PaymentDbContext()
    {
    }

    public PaymentDbContext(DbContextOptions<PaymentDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DBConnectionstring");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Payments_3214EC07ADCB71C2");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.Payments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payments_PaymentMethodId__440B1D62");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_PaymentMethods_3214EC07ADCB71C2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
