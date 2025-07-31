using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebAPIManagement.Models;

public partial class ErpSystemContext : DbContext
{
    public ErpSystemContext()
    {
    }

    public ErpSystemContext(DbContextOptions<ErpSystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ReceiptDocument> ReceiptDocuments { get; set; }

    public virtual DbSet<ReceiptResource> ReceiptResources { get; set; }

    public virtual DbSet<Resource> Resources { get; set; }

    public virtual DbSet<Unit> Units { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReceiptDocument>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ReceiptD__3214EC0741207192");

            entity.ToTable("ReceiptDocument");

            entity.HasIndex(e => e.Number, "UQ__ReceiptD__78A1A19DBA8064F8").IsUnique();

            entity.Property(e => e.Number).HasMaxLength(50);
        });

        modelBuilder.Entity<ReceiptResource>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ReceiptR__3214EC07FE63D11D");

            entity.ToTable("ReceiptResource");

            entity.Property(e => e.Quantity).HasColumnType("decimal(15, 3)");

            entity.HasOne(d => d.ReceiptDocument).WithMany(p => p.ReceiptResources)
                .HasForeignKey(d => d.ReceiptDocumentId)
                .HasConstraintName("FK_ReceiptResource_ReceiptDocument");

            entity.HasOne(d => d.Resource).WithMany(p => p.ReceiptResources)
                .HasForeignKey(d => d.ResourceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReceiptResource_Resource");

            entity.HasOne(d => d.Unit).WithMany(p => p.ReceiptResources)
                .HasForeignKey(d => d.UnitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReceiptResource_Unit");
        });

        modelBuilder.Entity<Resource>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Resource__3214EC07CE771952");

            entity.ToTable("Resource");

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(150);
        });

        modelBuilder.Entity<Unit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Unit__3214EC07157282CF");

            entity.ToTable("Unit");

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
