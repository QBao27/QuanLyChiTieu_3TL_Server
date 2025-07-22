using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace QuanLyChiTieu_3TL.Data;

public partial class QuanLyChiTieu3tlContext : DbContext
{
    public QuanLyChiTieu3tlContext()
    {
    }

    public QuanLyChiTieu3tlContext(DbContextOptions<QuanLyChiTieu3tlContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DanhMuc> DanhMucs { get; set; }

    public virtual DbSet<GiaoDich> GiaoDiches { get; set; }

    public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DanhMuc>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DanhMuc__3214EC07A011E51C");

            entity.ToTable("DanhMuc");

            entity.Property(e => e.Icon).HasMaxLength(50);
            entity.Property(e => e.LoaiDanhMuc).HasMaxLength(10);
            entity.Property(e => e.TenDanhMuc).HasMaxLength(100);
        });

        modelBuilder.Entity<GiaoDich>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GiaoDich__3214EC07CDE51CC7");

            entity.ToTable("GiaoDich");

            entity.Property(e => e.Color).HasMaxLength(50);
            entity.Property(e => e.LoaiGiaoDich).HasMaxLength(10);
            entity.Property(e => e.MoTa).HasMaxLength(255);
            entity.Property(e => e.SoTien).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.IdDanhMucNavigation).WithMany(p => p.GiaoDiches)
                .HasForeignKey(d => d.IdDanhMuc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GiaoDich__IdDanh__4E88ABD4");

            entity.HasOne(d => d.IdTaiKhoanNavigation).WithMany(p => p.GiaoDiches)
                .HasForeignKey(d => d.IdTaiKhoan)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GiaoDich__IdTaiK__4F7CD00D");
        });

        modelBuilder.Entity<TaiKhoan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TaiKhoan__3214EC074873427D");

            entity.ToTable("TaiKhoan");

            entity.HasIndex(e => e.Email, "UQ__TaiKhoan__A9D10534E0599D9B").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.MatKhau).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(20);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
