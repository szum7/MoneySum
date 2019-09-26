using Microsoft.EntityFrameworkCore;
using System;

namespace MoneyStats.DAL
{
    public class TableModelExample
    {

    }

    public class DBContext : DbContext
    {
        public DbSet<TableModelExample> TableModelExample { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.

                //optionsBuilder.UseSqlServer(@"Server=.\;Database=Hostable;Trusted_Connection=True;");
                optionsBuilder.UseSqlServer(@"Server=tcp:hostable.database.windows.net,1433;Initial Catalog=Hostable;Persist Security Info=False;User ID=admn;Password=1host2ABLE3;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }
    }
}
