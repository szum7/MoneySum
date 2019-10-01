namespace MoneyStats.DAL.Model
{
    public class TransactionTagConnModel
    {
        public decimal Id { get; set; }
        public decimal TransactionId { get; set; }
        public decimal TagId { get; set; }

        public TransactionModel Transaction { get; set; }
        public TagModel Tag { get; set; }
    }
}
