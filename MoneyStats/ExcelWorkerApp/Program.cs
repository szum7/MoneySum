using MoneyStats.BL;
using System;

namespace ExcelWorkerApp
{
    class Program
    {
        static void AwaitToEditMergedFileProgram(string outputText)
        {
            Console.WriteLine(outputText);
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
        }

        static void Main(string[] args)
        {
            var run = new ProgramRunner();
            var now = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            var nowString = "2019-11-03-8-52-36";
            const string bankFilesPath = @"C:\Users\Shy\Documents\Ego\AllDocs\bank";
            const string mergedFilePath = @"C:\Users\Shy\Documents\Ego\AllDocs\bank\Merged\Merged_";

            Console.WriteLine("" +
                "[1] CREATE MERGED FILE:\n" +
                "1. Read bank files.\n" +
                "2. Create new merged file.\n\n" +

                "[2] READ MERGED FILE AND REBUILD:\n" +
                "4. Read (edited) merged file.\n" +
                "5. Clear database. \n" +
                "6. Write to database.\n\n" +

                "[3] CLEAR AND REBUILD:\n" +
                "1. Read bank files.\n" +
                "2. Create new merged file.\n" +
                "3. Await user to edit the merged file.\n" +
                "4. Read edited merged file.\n" +
                "5. Clear database. \n" +
                "6. Write to database.\n\n" +

                "[4] ADD TO DATABASE: (unavailable)\n" +
                "1. Read bank files.\n" +
                "2. Add not-yet-existing transactions to a new merged file.\n" +
                "3. Await user to edit the merged file.\n" +
                "4. Add transactions from the merged file to the database.\n\n" +

                "[exit] EXIT PROGRAM");

            commandReading:
            string command = Console.ReadLine();
            switch (command.ToLower())
            {
                case "1":
                    run.ReadManyBankExportedFiles(bankFilesPath, "*.xls");
                    run.TruncateBankExportedFiles();
                    run.CreateMergedExcelFile($"{mergedFilePath}{now}.xls"); 
                    break;
                case "2":                    
                    run.ReadExtendedTransactionsMergedFile($"{mergedFilePath}{now}.xls");
                    run.ClearTransactionRelatedDataFromDatabase();
                    run.SaveBankExportedTransactions();
                    break;
                case "3":
                    run.ReadManyBankExportedFiles(bankFilesPath, "*.xls");
                    run.TruncateBankExportedFiles();
                    run.CreateMergedExcelFile($"{mergedFilePath}{now}.xls");
                    AwaitToEditMergedFileProgram("" +
                        "[Done] 1. Bank files read.\n" +
                        "[Done] 2. Merged file created.\n" +
                        "3. Awaiting user to edit the merged file.\n" +
                        "Edit the merged file and type OK to progress. Or type EXIT to exit.");
                    run.ReadExtendedTransactionsMergedFile($"{mergedFilePath}{now}.xls");
                    run.ClearTransactionRelatedDataFromDatabase();
                    run.SaveBankExportedTransactions();
                    break;
                case "4":
                    Console.WriteLine("Not yet implemented!");
                    break;
                case "exit":
                    Console.WriteLine("Program was exited.");
                    break;
                default:
                    Console.WriteLine("Command not supported!");
                    goto commandReading;
            }


            #region Test section
#if false
            var list = (new TransactionRepository()).Get();
            return;
#endif
            #endregion


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

#if false
            run.ReadExtendedTransactionsMergedFile($@"C:\Users\Shy\Documents\Ego\AllDocs\bank\Merged\Merged_{nowString}.xls");
            run.LoadAlreadyExistingTransactionsFromDatabase();
            run.MergeDatabaseAndExtendedMergedTransactions();
            run.SaveNewBankExportedTransactions();
#endif

            Console.WriteLine("PROGRAM ENDED.");
        }
    }
}
