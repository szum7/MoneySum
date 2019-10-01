namespace MoneyStats.DAL.Model
{
    public class TransactionTagConnModel
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public int TagId { get; set; }

        public TransactionModel Transaction { get; set; }
        public TagModel Tag { get; set; }
    }
}
