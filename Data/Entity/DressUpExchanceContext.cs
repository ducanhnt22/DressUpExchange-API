using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DressUpExchange.Data.Entity
{
    public partial class DressUpExchanceContext : DbContext
    {
        public DressUpExchanceContext()
        {
        }

        public DressUpExchanceContext(DbContextOptions<DressUpExchanceContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Notification> Notifications { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderItem> OrderItems { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductFeedback> ProductFeedbacks { get; set; } = null!;
        public virtual DbSet<ProductImage> ProductImages { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserSavedVoucher> UserSavedVouchers { get; set; } = null!;
        public virtual DbSet<Voucher> Vouchers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server =(local); database = DressUpExchance;uid=sa;pwd=12345;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CategoryName).HasMaxLength(50);

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.Property(e => e.NotificationId).HasColumnName("NotificationID");

                entity.Property(e => e.Message).HasColumnType("text");

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Notificat__UserI__5535A963");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.OrderDate).HasColumnType("date");

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Orders__UserID__4222D4EF");
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.Property(e => e.OrderItemId).HasColumnName("OrderItemID");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UserSavedVoucherId).HasColumnName("UserSavedVoucherID");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK__OrderItem__Order__4D94879B");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK__OrderItem__Produ__4E88ABD4");

                entity.HasOne(d => d.UserSavedVoucher)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.UserSavedVoucherId)
                    .HasConstraintName("FK__OrderItem__UserS__4CA06362");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK__Products__Catego__3C69FB99");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Products__UserID__3B75D760");
            });

            modelBuilder.Entity<ProductFeedback>(entity =>
            {
                entity.HasKey(e => e.FeedbackId)
                    .HasName("PK__ProductF__6A4BEDF6BC362617");

                entity.ToTable("ProductFeedback");

                entity.Property(e => e.FeedbackId).HasColumnName("FeedbackID");

                entity.Property(e => e.Comment).HasColumnType("text");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductFeedbacks)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK__ProductFe__Produ__52593CB8");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ProductFeedbacks)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__ProductFe__UserI__5165187F");
            });

            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.HasKey(e => e.ImageId)
                    .HasName("PK__ProductI__7516F4EC68C970FA");

                entity.Property(e => e.ImageId).HasColumnName("ImageID");

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(100)
                    .HasColumnName("ImageURL");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductImages)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK__ProductIm__Produ__3F466844");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.Address).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Password)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Role)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RefreshToken)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserSavedVoucher>(entity =>
            {
                entity.Property(e => e.UserSavedVoucherId).HasColumnName("UserSavedVoucherID");

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.VoucherId).HasColumnName("VoucherID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserSavedVouchers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__UserSaved__UserI__48CFD27E");

                entity.HasOne(d => d.Voucher)
                    .WithMany(p => p.UserSavedVouchers)
                    .HasForeignKey(d => d.VoucherId)
                    .HasConstraintName("FK__UserSaved__Vouch__49C3F6B7");
            });

            modelBuilder.Entity<Voucher>(entity =>
            {
                entity.Property(e => e.VoucherId).HasColumnName("VoucherID");

                entity.Property(e => e.Code).HasMaxLength(20);

                entity.Property(e => e.DiscountAmount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.ExpireDate).HasColumnType("date");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Vouchers)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK__Vouchers__Produc__45F365D3");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Vouchers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Vouchers__UserID__44FF419A");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
