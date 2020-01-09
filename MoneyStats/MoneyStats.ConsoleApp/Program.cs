using MoneyStats.BL;
using System;

namespace MoneyStats.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var transactionRepo = new TransactionRepository();
            var transactionRepoResult = transactionRepo.GetTransactionStats();
            Console.WriteLine("END GetTransactionStats");

            var tagRepo = new TagRepository();
            var tagRepoResult = tagRepo.GetAllTagDetailedSummary();
            Console.WriteLine("END GetAllTagDetailedSummary");

            Console.WriteLine("Program ended.");
        }
    }
}
