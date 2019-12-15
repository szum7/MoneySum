using Microsoft.EntityFrameworkCore;
using MoneyStats.DAL;
using MoneyStats.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyStats.BL
{
    public class TransactionTagConnRepository
    {
        public void Save(List<TransactionTagConn> tags)
        {
            using (var context = new MoneyStatsContext())
            {
                context.TransactionTagConn.AddRange(tags);
                context.SaveChanges();
            }
        }

        public void DeleteAll()
        {
            using (var context = new MoneyStatsContext())
            {
                context.Database.ExecuteSqlCommand("delete from dbo.[TransactionTagConn];DBCC CHECKIDENT ([TransactionTagConn], RESEED, 0);");
                context.SaveChanges();
            }
        }
    }
}
