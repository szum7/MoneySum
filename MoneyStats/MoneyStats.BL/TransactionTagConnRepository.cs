using MoneyStats.DAL;
using MoneyStats.DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyStats.BL
{
    public class TransactionTagConnRepository
    {
        public void Save(List<TransactionTagConnModel> tags)
        {
            using (var context = new DBContext())
            {
                context.TransactionTagConn.AddRange(tags);
                context.SaveChanges();
            }
        }
    }
}
