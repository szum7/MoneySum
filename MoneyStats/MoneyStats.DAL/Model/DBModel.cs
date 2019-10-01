using System;

namespace MoneyStats.DAL.Model
{
    public abstract class DBModel
    {
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string State { get; set; }

    }
}
