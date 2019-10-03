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
            var excelReader = new ExcelReader<Transaction>();
            var transactionRepo = new TransactionRepository();
            var mergedFileReader = new ExcelReader<TransactionExtended>();
            var transactionMerger = new TransactionMerger();


            // Test section
#if false
            var list = transactionRepo.GetWithEntities();
            return;
#endif

            // Read many bank-exported excel files
#if false
            ExcelSheet<Transaction> allFiles = excelReader.Read(@"C:\Users\Aron_Szocs\Documents\Bank", "*.xls");
#endif

            // Merge read files
#if false
            excelReader.TruncateData();
#endif

            // Write merged file to an excel file
#if false
            ExcelWriter excelWriter = new ExcelWriter();
            excelWriter.Run(allFiles, @"C:\Users\Aron_Szocs\Documents\Bank\Merged\Merged.xls");
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

            // Read one merged excel file
#if true            
            mergedFileReader.IsReadFromTheBeginning = true;
            var mergedFile = mergedFileReader.Read(@"C:\Users\Aron_Szocs\Documents\Bank\Merged\Merged.xls");
#endif

            // Load already existing transactions from database
#if true
            var transactionsFromDB = transactionRepo.GetWithEntities();
#endif

            // Merge db data and merged-excel file data
#if true            
            var mergedList = transactionMerger.Run(transactionsFromDB, mergedFile.Transactions);
            var unsavedTransactionList = transactionMerger.GetNewRows(mergedList);
#endif

            // Write to db
#if true
            transactionRepo.SmartSave(unsavedTransactionList);
#endif

            Console.WriteLine("PROGRAM ENDED.");
        }
    }
}
