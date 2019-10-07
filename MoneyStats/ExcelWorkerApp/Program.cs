using ExcelWorkerApp.Components.MergeTransaction;
using ExcelWorkerApp.Components.ReadExcel;
using ExcelWorkerApp.Components.WriteExcel;
using ExcelWorkerApp.Model;
using ExcelWorkerApp.Test;
using MoneyStats.BL;
using System;

namespace ExcelWorkerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Test section
#if false
            var list = (new MergeTest()).MergeTestMethod();
            return;
#endif

            var run = new ProgramRunner();

            run.ReadManyBankExportedFiles(@"C:\Users\Aron_Szocs\Documents\Bank", "*.xls");
            run.TruncateBankExportedFiles();
            run.CreateMergedExcelFile(@"C:\Users\Aron_Szocs\Documents\Bank\Merged\Merged.xls");

            // => User edits the file
#if true
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

            run.ReadExtendedTransactionsMergedFile(@"C:\Users\Aron_Szocs\Documents\Bank\Merged\Merged.xls");
            run.LoadAlreadyExistingTransactionsFromDatabase();
            run.MergeDatabaseAndExtendedMergedTransactions();
            run.SaveNewBankExportedTransactions();

            Console.WriteLine("PROGRAM ENDED.");
        }
    }
}
