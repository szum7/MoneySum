using System;
using System.Collections.Generic;
using System.Linq;

namespace MoneyStats.BL.Utility
{
    public class Common
    {
        // TODO test if from and to is in the array + test if all x.01
        public static List<DateTime> GetMonthsList(DateTime from, DateTime to)
        {
            var months = new List<DateTime>();

            for (var dt = from; dt <= to; dt = dt.AddMonths(1))
            {
                months.Add(dt);
            }

            return months;
        }
    }
}
