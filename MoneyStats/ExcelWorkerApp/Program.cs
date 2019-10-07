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
            var excelReader = new ExcelReader<ExcelTransaction>();
            var excelWriter = new ExcelWriter();
            var transactionRepo = new TransactionRepository();
            var mergedFileReader = new ExcelReader<ExcelTransactionExtended>();
            var transactionMerger = new TransactionMerger();


            // Test section
#if false
            var list = (new MergeTest()).MergeTestMethod();
            return;
#endif

            // Read many bank-exported excel files
#if true
            ExcelSheet<ExcelTransaction> allFiles = excelReader.Read(@"C:\Users\Aron_Szocs\Documents\Bank", "*.xls");
#endif

            // Merge read files
#if true
            excelReader.TruncateData(allFiles);
#endif

            // Write merged file to an excel file
#if true
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
            excelWriter.Run(mergedList, @"C:\Users\Aron_Szocs\Documents\Bank\Merged\MergedList.xls");
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
