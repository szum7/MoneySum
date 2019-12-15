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

            Console.WriteLine("" +
                "[1] CLEAR AND REBUILD: (unavailable)\n" +
                "1. Read bank files.\n" +
                "2. Create new merged file.\n" +
                "3. Await user to edit the merged file.\n" +
                "4. Read edited merged file.\n" +
                "5. Clear database. \n" +
                "6. Write to database.\n\n" +

                "[2] ADD TO DATABASE: (unavailable)\n" +
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
                    run.ReadManyBankExportedFiles(@"C:\Users\Shy\Documents\Ego\AllDocs\bank", "*.xls");
                    run.TruncateBankExportedFiles();
                    run.CreateMergedExcelFile($@"C:\Users\Shy\Documents\Ego\AllDocs\bank\Merged\Merged_{now}.xls");
                    AwaitToEditMergedFileProgram("" +
                        "1. [Done] Bank files read.\n" +
                        "2. [Done] Merged file created.\n" +
                        "3. Awaiting user to edit the merged file.\n" +
                        "Edit the merged file and type OK to progress. Or type EXIT to exit.");
                    run.ReadExtendedTransactionsMergedFile($@"C:\Users\Shy\Documents\Ego\AllDocs\bank\Merged\Merged_{now}.xls");
                    run.ClearTransactionRelatedDataFromDatabase();                    
                    run.SaveBankExportedTransactions();

                    break;
                case "2":
                    break;
                case "exit":
                    Console.WriteLine("Program was exited.");
                    break;
                default:
                    Console.WriteLine("Command not supported!");
                    goto commandReading;
            }


            // Test section
#if true
            var list = (new TransactionRepository()).Get();
            return;
#endif

            

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
