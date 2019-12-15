using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using MoneyStats.DAL.Models;

namespace MoneyStats.DAL
{
    public partial class MoneyStatsContext : DbContext
    {
        public MoneyStatsContext()
        {
        }

        public MoneyStatsContext(DbContextOptions<MoneyStatsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Currency> Currency { get; set; }
        public virtual DbSet<Setting> Setting { get; set; }
        public virtual DbSet<Tag> Tag { get; set; }
        public virtual DbSet<Transaction> Transaction { get; set; }
        public virtual DbSet<TransactionTagConn> TransactionTagConn { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=MoneyStats.Database;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
                //optionsBuilder.UseSqlServer("Server=.\\;Database=MoneyStats;Trusted_Connection=True;");
                //Data Source=(localdb)\ProjectsV13;Initial Catalog=MoneyStats.Database;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Currency>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK_CURR_ID")
                    .IsClustered(false);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(true);
            });

            modelBuilder.Entity<Setting>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK_SETTI_ID")
                    .IsClustered(false);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.DbexportFilename)
                    .HasColumnName("DBExportFilename")
                    .HasMaxLength(500);

                entity.Property(e => e.DbexportFolderPath)
                    .HasColumnName("DBExportFolderPath")
                    .HasMaxLength(500);

                entity.Property(e => e.DbimportFilePath)
                    .HasColumnName("DBImportFilePath")
                    .HasMaxLength(500);

                entity.Property(e => e.MergedFileFolderPath).HasMaxLength(500);

                entity.Property(e => e.MergedFilename).HasMaxLength(500);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.OriginalFileExtensionPattern).HasMaxLength(500);

                entity.Property(e => e.OriginalFileFolderPath).HasMaxLength(500);

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(true);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Setting)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_USER_ID");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK_TAGT_ID")
                    .IsClustered(false);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(true);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(true);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(true);
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK_TRAN_ID")
                    .IsClustered(false);

                entity.Property(e => e.Account).HasMaxLength(255);

                entity.Property(e => e.AccountName).HasMaxLength(255);

                entity.Property(e => e.AccountingDate).HasColumnType("datetime");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Message).HasMaxLength(500);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.PartnerAccount).HasMaxLength(255);

                entity.Property(e => e.PartnerName).HasMaxLength(255);

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(true);

                entity.Property(e => e.Sum).HasColumnType("numeric(15, 2)");

                entity.Property(e => e.TransactionId).HasMaxLength(255);

                entity.Property(e => e.Type).HasMaxLength(255);

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.Transaction)
                    .HasForeignKey(d => d.CurrencyId)
                    .HasConstraintName("FK_TRAN_CURRENCY_ID");
            });

            modelBuilder.Entity<TransactionTagConn>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK_TTCT_ID")
                    .IsClustered(false);

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.TransactionTagConn)
                    .HasForeignKey(d => d.TagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TAG_ID");

                entity.HasOne(d => d.Transaction)
                    .WithMany(p => p.TransactionTagConn)
                    .HasForeignKey(d => d.TransactionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TRAN_ID");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK_USER_ID")
                    .IsClustered(false);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(true);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
