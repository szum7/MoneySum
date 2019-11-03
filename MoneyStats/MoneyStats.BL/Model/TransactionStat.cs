using MoneyStats.DAL.Models;
using System.Collections.Generic;

namespace MoneyStats.BL.Model
{
    public class TransactionStat
    {
        public List<Transaction> Transactions { get; set; }
        public List<MonthStat> MonthStats { get; set; }

        public TransactionStat()
        {
            this.Transactions = new List<Transaction>();
            this.MonthStats = new List<MonthStat>();
        }
    }
}
