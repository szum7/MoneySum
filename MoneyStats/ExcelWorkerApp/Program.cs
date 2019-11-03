using MoneyStats.BL;
using System;

namespace ExcelWorkerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Test section
#if true
            var list = (new TransactionRepository()).Get();
            return;
#endif

            var run = new ProgramRunner();
            var now = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            var nowString = "2019-11-03-8-52-36";

#if false
            run.ReadManyBankExportedFiles(@"C:\Users\Shy\Documents\Ego\AllDocs\bank", "*.xls");
            run.TruncateBankExportedFiles();
            run.CreateMergedExcelFile($@"C:\Users\Shy\Documents\Ego\AllDocs\bank\Merged\Merged_{nowString}.xls");
#endif

            // => User edits the file
#if false
            Console.WriteLine("========================");
            Console.WriteLine("USER INTERACTION NEEDED!");
            Console.WriteLine("========================");
            Console.WriteLine("You should edit the merged file now. Type 'OK' to read the merged file, or 'EXIT' to exit the program.");
            string input = "";
            while (input.ToLower() != "ok")
            {
                input = Console.ReadLine();
                if (input.ToLower() == "exit")
                {
                    Console.WriteLine("Program exited.");
                    return;
                }
            }
#endif

#if true
            run.ReadExtendedTransactionsMergedFile($@"C:\Users\Shy\Documents\Ego\AllDocs\bank\Merged\Merged_{nowString}.xls");
            run.LoadAlreadyExistingTransactionsFromDatabase();
            run.MergeDatabaseAndExtendedMergedTransactions();
            run.SaveNewBankExportedTransactions();
#endif

            Console.WriteLine("PROGRAM ENDED.");
        }
    }
}
