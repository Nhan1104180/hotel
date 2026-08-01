using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RoomDBService.src.Domain.Entities;

namespace RoomDBService.src.Infrastructure.Data;

public partial class RoomDbContext : DbContext
{
    public RoomDbContext()
    {
    }

    public RoomDbContext(DbContextOptions<RoomDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<RoomImage> RoomImages { get; set; }

    public virtual DbSet<RoomType> RoomTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DBConnectionstring");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Rooms_3214EC07ADCB71C2");

            entity.HasOne(d => d.RoomType).WithMany(p => p.Rooms)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Rooms__RoomTypeI__35BCFE0A");
        });

        modelBuilder.Entity<RoomImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_RoomImages_3214EC07ADCB71C2");

            entity.HasOne(d => d.Room).WithMany(p => p.RoomImages)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RoomImage__RoomI__398D8EEE");
        });

        modelBuilder.Entity<RoomType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_RoomTypes_3214EC07ADCB71C2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
