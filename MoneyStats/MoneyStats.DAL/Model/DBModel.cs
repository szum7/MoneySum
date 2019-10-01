using System;

namespace MoneyStats.DAL.Model
{
    public abstract class DBModel
    {
        public int ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string State { get; set; }

    }
}
