using Microsoft.EntityFrameworkCore;
using MoneyStats.DAL.Model;
using System;

namespace MoneyStats.DAL
{
    public class DBContext : DbContext
    {
        public DbSet<CurrencyModel> CURRENCY { get; set; }
        public DbSet<SettingModel> SETTING { get; set; }
        public DbSet<TagModel> TAG { get; set; }
        public DbSet<TransactionModel> TRANSACTION { get; set; }
        public DbSet<UserModel> USER { get; set; }
        public DbSet<TransactionTagConnModel> TRANSACTION_TAG_CONN { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.

                optionsBuilder.UseSqlServer(@"Server=.\;Database=MoneyStats;Trusted_Connection=True;");
                //optionsBuilder.UseSqlServer(@"Server=tcp:hostable.database.windows.net,1433;Initial Catalog=Hostable;Persist Security Info=False;User ID=admn;Password=1host2ABLE3;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }
    }
}
