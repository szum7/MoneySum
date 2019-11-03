using System;

namespace MoneyStats.BL.Model
{
    public class MonthStat
    {
        public DateTime Date { get; set; }
        public decimal? Income { get; set; }
        public decimal? Expense { get; set; }
        public decimal? Flow
        {
            get
            {
                if (!Income.HasValue || !Expense.HasValue)
                    return null;
                return Income.Value - Math.Abs(Expense.Value);
            }
        }
    }
}
