using System;
using System.Collections.Generic;

namespace MoneyStats.BL.TagRepo.Model
{
    public class TagMonthSummary
    {
        public DateTime Month { get; set; }
        public List<TagMonthSummaryLine> Tags { get; set; }
        public List<TagInstance> TagInstances { get; set; } // Unused
    }
}
