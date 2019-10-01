using System;

namespace MoneyStats.DAL.Model
{
    public abstract class DBModel
    {
        public decimal ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public decimal CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string State { get; set; }

    }
}
