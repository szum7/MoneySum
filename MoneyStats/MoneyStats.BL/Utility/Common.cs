using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoneyStats.BL.Utility
{
    public class Common
    {
        public static List<DateTime> GetDatesList(DateTime from, DateTime to)
        {
            return Enumerable
                .Range(0, 1 + to.Subtract(from).Days)
                .Select(offset => from.AddDays(offset))
                .ToList();
        }
    }
}
