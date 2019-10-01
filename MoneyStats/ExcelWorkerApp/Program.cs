using ExcelWorkerApp.Components.MergeTransaction;
using ExcelWorkerApp.Components.ReadExcel;
using ExcelWorkerApp.Components.WriteExcel;
using ExcelWorkerApp.Model;
using MoneyStats.BL;
using System;

namespace ExcelWorkerApp
{
    class Program
    {
        static void Main(string[] args)
        {
#if false
            var repo = new TransactionRepository();
            var list = repo.GetWithEntities();
            return;
#endif

            // Read many bank-exported excel files
#if true
            ExcelReader<Transaction> reader = new ExcelReader<Transaction>();
            ExcelSheet<Transaction> allFiles = reader.Read(@"C:\Users\Aron_Szocs\Documents\Bank", "*.xls");
#endif

            // Merge read files
#if true
            reader.TruncateData();
#endif

            // Write merged file to an excel file
#if true
            ExcelWriter excelWriter = new ExcelWriter();
            excelWriter.Run(allFiles, @"C:\Users\Aron_Szocs\Documents\Bank\Merged\Merged.xls");
#endif

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

            // Read one merged excel file
#if true
            var mergedFileReader = new ExcelReader<TransactionExtended>();
            mergedFileReader.IsReadFromTheBeginning = true;
            var mergedFile = mergedFileReader.Read(@"C:\Users\Aron_Szocs\Documents\Bank\Merged\Merged.xls");
#endif

            // Load already existing transactions from database
#if true
            var repo = new TransactionRepository();
            var transactionsFromDB = repo.GetWithEntities();
#endif

            // Merge db data and merged-excel file data
#if true
            var merger = new TransactionMerger();
            var mergedList = merger.Run(transactionsFromDB, mergedFile.Transactions);
#endif

            // Write to db


            Console.WriteLine("PROGRAM ENDED.");
        }
    }
}
