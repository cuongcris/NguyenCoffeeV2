using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace NguyenCoffeeWeb.Models
{
    public partial class postgresContext : DbContext
    {
        public postgresContext()
        {
        }

        public postgresContext(DbContextOptions<postgresContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<History> Histories { get; set; } = null!;
        public virtual DbSet<ImageAi> ImageAis { get; set; } = null!;
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public virtual DbSet<OrderInTable> OrderInTables { get; set; } = null!;
        public virtual DbSet<OrdersOnline> OrdersOnlines { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("User Id=postgres.xzoexxdjquaucxpnzgez;Password=0976563934okK@;Server=aws-0-ap-southeast-1.pooler.supabase.com;Port=5432;Database=postgres;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresEnum("auth", "aal_level", new[] { "aal1", "aal2", "aal3" })
                .HasPostgresEnum("auth", "code_challenge_method", new[] { "s256", "plain" })
                .HasPostgresEnum("auth", "factor_status", new[] { "unverified", "verified" })
                .HasPostgresEnum("auth", "factor_type", new[] { "totp", "webauthn" })
                .HasPostgresEnum("auth", "one_time_token_type", new[] { "confirmation_token", "reauthentication_token", "recovery_token", "email_change_token_new", "email_change_token_current", "phone_change_token" })
                .HasPostgresEnum("pgsodium", "key_status", new[] { "default", "valid", "invalid", "expired" })
                .HasPostgresEnum("pgsodium", "key_type", new[] { "aead-ietf", "aead-det", "hmacsha512", "hmacsha256", "auth", "shorthash", "generichash", "kdf", "secretbox", "secretstream", "stream_xchacha20" })
                .HasPostgresEnum("realtime", "action", new[] { "INSERT", "UPDATE", "DELETE", "TRUNCATE", "ERROR" })
                .HasPostgresEnum("realtime", "equality_op", new[] { "eq", "neq", "lt", "lte", "gt", "gte", "in" })
                .HasPostgresExtension("extensions", "pg_stat_statements")
                .HasPostgresExtension("extensions", "pgcrypto")
                .HasPostgresExtension("extensions", "pgjwt")
                .HasPostgresExtension("extensions", "uuid-ossp")
                .HasPostgresExtension("graphql", "pg_graphql")
                .HasPostgresExtension("pgsodium", "pgsodium")
                .HasPostgresExtension("vault", "supabase_vault");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.HasIndex(e => e.Email, "Account_Email_key")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.FullName).HasColumnName("Full Name");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("now()");
            });

            modelBuilder.Entity<History>(entity =>
            {
                entity.ToTable("History");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("now()");
            });

            modelBuilder.Entity<ImageAi>(entity =>
            {
                entity.ToTable("ImageAi");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("now()");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ImageAis)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("ImageAi_UserId_fkey");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => new { e.ProductId, e.OrderId })
                    .HasName("OrderDetail_pkey");

                entity.ToTable("OrderDetail");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("public_OrderDetail_OrderId_fkey");

                entity.HasOne(d => d.OrderInTable)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderInTableId)
                    .HasConstraintName("public_OrderDetail_OrderInTableId_fkey");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("public_OrderDetail_ProductId_fkey");
            });

            modelBuilder.Entity<OrderInTable>(entity =>
            {
                entity.ToTable("OrderInTable");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("now()");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.OrderInTables)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("public_OrderInTable_UserId_fkey");
            });

            modelBuilder.Entity<OrdersOnline>(entity =>
            {
                entity.ToTable("OrdersOnline");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.PackagingDate)
                    .HasColumnType("timestamp without time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.ShippedDate)
                    .HasColumnType("timestamp without time zone")
                    .HasDefaultValueSql("now()");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.OrdersOnlines)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("public_Orders_UserId_fkey");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("now()");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("now()");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("public_Products_CategoryId_fkey");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
