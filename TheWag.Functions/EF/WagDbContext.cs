using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TheWag.Api.WagDB.EF;

public partial class WagDbContext : DbContext
{
    public WagDbContext()
    {
    }

    public WagDbContext(DbContextOptions<WagDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<Vendor> Vendors { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=tcp:wagsqlserver.database.windows.net,1433;Initial Catalog=WagDB;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication='Active Directory Default';");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customer");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Email)
                .HasMaxLength(75)
                .IsFixedLength();
            entity.Property(e => e.FirstName)
                .HasMaxLength(25)
                .IsFixedLength()
                .HasColumnName("First_Name");
            entity.Property(e => e.LastName)
                .HasMaxLength(25)
                .IsFixedLength()
                .HasColumnName("Last_Name");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Order");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.FkCustomerId).HasColumnName("FK_Customer_ID");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Order)
                .HasForeignKey<Order>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Customer");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.ToTable("Order_Details");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.FkOrderId).HasColumnName("FK_ORDER_ID");
            entity.Property(e => e.FkProductId).HasColumnName("FK_Product_ID");

            entity.HasOne(d => d.FkOrder).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.FkOrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Details_Order");

            entity.HasOne(d => d.FkProduct).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.FkProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Details_Products");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.FkVendorId).HasColumnName("FK_VENDOR_ID");
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.Url)
                .HasMaxLength(255)
                .IsFixedLength()
                .HasColumnName("URL");

            entity.HasOne(d => d.FkVendor).WithMany(p => p.Products)
                .HasForeignKey(d => d.FkVendorId)
                .HasConstraintName("FK_Products_Vendor");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.FkProductId).HasColumnName("FK_Product_ID");
            entity.Property(e => e.Text)
                .HasMaxLength(255)
                .IsFixedLength()
                .HasColumnName("Text");

            entity.HasOne(d => d.FkProduct).WithMany(p => p.Tags)
                .HasForeignKey(d => d.FkProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tags_Products");
        });

        modelBuilder.Entity<Vendor>(entity =>
        {
            entity.ToTable("Vendor");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Email)
                .HasMaxLength(75)
                .IsFixedLength();
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
