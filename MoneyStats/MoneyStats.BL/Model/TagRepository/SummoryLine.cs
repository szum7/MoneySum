using MoneyStats.DAL.Models;
using System;
using System.Collections.Generic;

namespace MoneyStats.BL.Model.TagRepository
{
    public abstract class SummoryLine
    {
        public DateTime Month { get; set; }
        public decimal Income { get; set; }
        public decimal Expense { get; set; }
        public decimal Flow { get { return Income - Math.Abs(Expense); } }
        public List<Transaction> Transactions { get; set; }
    }
}
