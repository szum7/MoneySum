using Microsoft.EntityFrameworkCore;
using MoneyStats.DAL.Model;
using System;

namespace MoneyStats.DAL
{
    //public class DBContext : DbContext
    //{
    //    public DbSet<CurrencyModel> Currency { get; set; }
    //    //public DbSet<SettingModel> Setting { get; set; } // unused
    //    public DbSet<TagModel> Tag { get; set; }
    //    public DbSet<Transaction> Transaction { get; set; }
    //    public DbSet<UserModel> User { get; set; }
    //    public DbSet<TransactionTagConnModel> TransactionTagConn { get; set; }

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    {
    //        if (!optionsBuilder.IsConfigured)
    //        {
    //            optionsBuilder.UseSqlServer(@"Server=.\;Database=MoneyStats;Trusted_Connection=True;");
    //        }
    //    }
    //}
}
