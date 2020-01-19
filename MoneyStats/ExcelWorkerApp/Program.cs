using MoneyStats.BL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

        static void ChangeMergedFileNameProgram(string defaultName, out string mergedFileName)
        {
            Console.Write("Write the new name of the merged file: ");
            var command = Console.ReadLine();

            switch (command.Trim())
            {
                case "":
                    mergedFileName = defaultName;
                    break;
                case "default":
                    mergedFileName = defaultName;
                    break;
                default:
                    mergedFileName = command;
                    break;
            }
            Console.WriteLine("Merged filename changed to: " + mergedFileName);
        }

        static void ChangeMergedFilePathProgram(string defaultName, out string bankFilesPath)
        {
            Console.Write("Write the new name of the bankFilesPath: ");
            var command = Console.ReadLine();

            switch (command.Trim())
            {
                case "":
                    bankFilesPath = defaultName;
                    break;
                case "default":
                    bankFilesPath = defaultName;
                    break;
                default:
                    bankFilesPath = command;
                    break;
            }
            Console.WriteLine("BankFilesPath changed to: " + bankFilesPath);
        }

        static void ChangeBankFilesPathProgram(string defaultName, out string mergedFilePath)
        {
            Console.Write("Write the new name of the mergedFilePath: ");
            var command = Console.ReadLine();

            switch (command.Trim())
            {
                case "":
                    mergedFilePath = defaultName;
                    break;
                case "default":
                    mergedFilePath = defaultName;
                    break;
                default:
                    mergedFilePath = command;
                    break;
            }
            Console.WriteLine("MergedFilePath changed to: " + mergedFilePath);
        }

        static void Main(string[] args)
        {
            var run = new ProgramRunner();
            var now = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            var staticNow = "2019-11-03-8-52-36";
            var mergedFileName = $"Merged_{now}";
            const string orignalBankFilesPath = @"C:\Users\Shy\Documents\Ego\AllDocs\bank";
            var bankFilesPath = @"C:\Users\Shy\Documents\Ego\AllDocs\bank";
            const string orignalMergedFilePath = @"C:\Users\Shy\Documents\Ego\AllDocs\bank\Merged\";
            var mergedFilePath = @"C:\Users\Shy\Documents\Ego\AllDocs\bank\Merged\";

            var programInfo = "" +
                "Commands are listed below in brackets []. For example: [2] -> type the number 2 and hit enter to run that program.\n\n" + 
                "[merged] Update the name (without extension) of the merged file for this run. Default is {Merged_" + now + "}\n" +
                "[bankFilesPath] Update the path of the bank files for this run. Default is {" + bankFilesPath + "}\n" +
                "[mergedFilePath] Update the path of the bank files for this run. Default is {" + mergedFilePath + "}\n" +
                "[!] Show program instructions. (Useful if console got scrolled away.)\n" +
                "[?] Get the list of local variables.\n\n" +

                "[1] CREATE MERGED FILE:\n" +
                "1. Read bank files.\n" +
                "2. Create new merged file.\n\n" +

                "[2] READ MERGED FILE AND REBUILD:\n" +
                "4. Read (edited) merged file.\n" +
                "5. Clear database. \n" +
                "6. Write to database.\n\n" +

                "[3] CLEAR, CREATE AND REBUILD:\n" +
                "1. Read bank files.\n" +
                "2. Create new merged file.\n" +
                "3. Await user to edit the merged file.\n" +
                "4. Read edited merged file.\n" +
                "5. Clear database. \n" +
                "6. Write to database.\n\n" +

                "[4] ADD TO MERGED:\n" +
                "1. Read bank files.\n" +
                "2. Merge read rows with the last merged file. (Last merged file rows unchanged.)\n\n" +

                "[5] ADD TO DATABASE: (unavailable)\n" +
                "1. Read bank files.\n" +
                "2. Merge read rows with rows from the database.\n" +
                "3. Create merged file.\n\n" +

                "[exit] EXIT PROGRAM";

            Console.WriteLine(programInfo);

            commandReading:
            Console.Write("Awaiting command: ");
            string command = Console.ReadLine();
            switch (command.ToLower())
            {
                #region case: Info programs
                case "merged":
                    ChangeMergedFileNameProgram($"Merged_{now}", out mergedFileName);
                    goto commandReading;
                case "bankFilesPath":
                    ChangeBankFilesPathProgram(orignalBankFilesPath, out bankFilesPath);
                    goto commandReading;
                case "mergedFilePath":
                    ChangeMergedFilePathProgram(orignalMergedFilePath, out mergedFilePath);
                    goto commandReading;
                case "?":
                    Console.WriteLine($"merged = {mergedFileName}");
                    Console.WriteLine($"bankFilesPath = {bankFilesPath}");
                    Console.WriteLine($"mergedFilePath = {mergedFilePath}");
                    goto commandReading;
                case "!":
                    Console.WriteLine(programInfo);
                    goto commandReading;
                #endregion

                #region case: Programs
                case "1":
                    run.ReadManyBankExportedFiles(bankFilesPath, "*.xls");
                    run.TruncateBankExportedFiles();
                    run.CreateMergedExcelFile($"{mergedFilePath}{mergedFileName}.xls"); 
                    break;
                case "2":                    
                    run.ReadAndHandleExtendedTransactionsMergedFile($"{mergedFilePath}{mergedFileName}.xls");
                    run.ClearTransactionRelatedDataFromDatabase();
                    run.SaveBankExportedTransactions();
                    break;
                case "3":
                    run.ReadManyBankExportedFiles(bankFilesPath, "*.xls");
                    run.TruncateBankExportedFiles();
                    run.CreateMergedExcelFile($"{mergedFilePath}{mergedFileName}.xls");
                    AwaitToEditMergedFileProgram("" +
                        "[Done] 1. Bank files read.\n" +
                        "[Done] 2. Merged file created.\n" +
                        "3. Awaiting user to edit the merged file.\n" +
                        "Edit the merged file and type OK to progress. Or type EXIT to exit.");
                    run.ReadAndHandleExtendedTransactionsMergedFile($"{mergedFilePath}{mergedFileName}.xls");
                    run.ClearTransactionRelatedDataFromDatabase();
                    run.SaveBankExportedTransactions();
                    break;
                case "4":
                    // TODO!! Ebben a programban a contentId összehasonlításnál nem szabad figyelembe venni a kiegészített sorokat! (Az "IsOmitted", "Tag names", stb. oszlopokat!)
                    run.ReadManyBankExportedFiles(bankFilesPath, "*.xls");
                    run.TruncateBankExportedFiles();
                    // read last merged file
                    run.ReadLastExtendedTransactionsMergedFile(mergedFilePath);
                    // merge last merged file's content (extendedExcel) with newly read rows (excel)
                    run.MergeLastExtendedTransactionsWithNewlyReadOnes();
                    // create new file
                    run.CreateExtendedMergedExcelFile($"{mergedFilePath}{mergedFileName}.xls");
                    break;
                case "5":
                    // Might be too complicated. I can't really create excel rows from db rows -> tagGroupId would not make sense!
                    Console.WriteLine("Not yet implemented!");
                    break;
                #endregion

                #region case: Instruction programs
                case "exit":
                    Console.WriteLine("Program was exited.");
                    break;
                default:
                    Console.WriteLine("Command not supported!");
                    goto commandReading;
                #endregion
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
