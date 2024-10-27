using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AppServer.Models;

public partial class SweetBoxDBContext : DbContext
{
    public SweetBoxDBContext()
    {
    }

    public SweetBoxDBContext(DbContextOptions<SweetBoxDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Buyer> Buyers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Seller> Sellers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server = (localdb)\\MSSQLLocalDB;Initial Catalog=SweetBox_DB;User ID=SweetBoxAdminLogin;Password=kukuPassword;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Admins__1788CC4CE7D394F8");

            entity.Property(e => e.UserId).ValueGeneratedNever();
            entity.Property(e => e.AdminLevel).HasDefaultValue(1);

            entity.HasOne(d => d.User).WithOne(p => p.Admin)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Admins__UserId__2F10007B");
        });

        modelBuilder.Entity<Buyer>(entity =>
        {
            entity.HasKey(e => e.BuyerId).HasName("PK__Buyers__4B81C62A2F364562");

            entity.Property(e => e.BuyerId).ValueGeneratedNever();

            entity.HasOne(d => d.BuyerNavigation).WithOne(p => p.Buyer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Buyers__BuyerId__2B3F6F97");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BCFC4737AEC");

            entity.Property(e => e.OrderDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Buyer).WithMany(p => p.Orders).HasConstraintName("FK__Orders__BuyerId__36B12243");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.OrderItemId).HasName("PK__OrderIte__57ED06816D6A6D3A");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems).HasConstraintName("FK__OrderItem__Order__3A81B327");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderItems).HasConstraintName("FK__OrderItem__Produ__3B75D760");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6CDC3E0E7B3");

            entity.Property(e => e.DateAdded).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsAvailable).HasDefaultValue(true);

            entity.HasOne(d => d.Seller).WithMany(p => p.Products).HasConstraintName("FK__Products__Seller__31EC6D26");
        });

        modelBuilder.Entity<Seller>(entity =>
        {
            entity.HasKey(e => e.SellerId).HasName("PK__Sellers__7FE3DB81171B7377");

            entity.Property(e => e.SellerId).ValueGeneratedNever();

            entity.HasOne(d => d.SellerNavigation).WithOne(p => p.Seller)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sellers_Users");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C903D497A");

            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
