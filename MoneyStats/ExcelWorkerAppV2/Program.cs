using ExcelWorkerApp.Model;
using MoneyStats.BL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExcelWorkerAppV2
{
    class Program
    {
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
            /*
             * 1. read bank files
             * 2. create merged file
             * 
             * 1. read bank files
             * 2. merge last merged file with new rows
             * 3. create merged file
             * 
             * 1. clear database
             * 
             * 1. read merged file
             * 2. merge read rows with database rows
             * 
             * 1. export database to merged file
             */

            var now = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            var mergedFileName = $"Merged_{now}";
            var bankFilesPath = @"C:\Users\Shy\Documents\Ego\AllDocs\bank";
            var mergedFilePath = @"C:\Users\Shy\Documents\Ego\AllDocs\bank\Merged\";
            const string filePattern = "*.xls";
            const string orignalBankFilesPath = @"C:\Users\Shy\Documents\Ego\AllDocs\bank";
            const string orignalMergedFilePath = @"C:\Users\Shy\Documents\Ego\AllDocs\bank\Merged\";

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

            ExcelReader reader = new ExcelReader();
            ExcelWriter writer = new ExcelWriter();
            TransactionConverter converter;
            ExcelSheet inMemoryExcelSheet;

            var transTagConnRepo = new TransactionTagConnRepository();
            var transRepo = new TransactionRepository();
            var tagRepo = new TagRepository();
            var currencyRepo = new CurrencyRepository();

            commandReading:
            Console.Write("Awaiting command: ");
            string command = Console.ReadLine();
            switch (command.ToLower())
            {
                #region case: Info programs
                case "merged":
                    ChangeMergedFileNameProgram($"Merged_{now}", out mergedFileName);
                    break;
                case "bankFilesPath":
                    ChangeBankFilesPathProgram(orignalBankFilesPath, out bankFilesPath);
                    break;
                case "mergedFilePath":
                    ChangeMergedFilePathProgram(orignalMergedFilePath, out mergedFilePath);
                    break;
                case "?":
                    Console.WriteLine($"merged = {mergedFileName}");
                    Console.WriteLine($"bankFilesPath = {bankFilesPath}");
                    Console.WriteLine($"mergedFilePath = {mergedFilePath}");
                    break;
                case "!":
                    Console.WriteLine(programInfo);
                    break;
                #endregion

                #region case: Programs
                case "1":

                    // 1. Read bank files
                    reader.IsReadFromTheBeginning = false;
                    inMemoryExcelSheet = reader.ReadFiles(bankFilesPath, filePattern);

                    // Remove duplicate rows
                    inMemoryExcelSheet.Truncate();

                    // 2. Create new merged file
                    writer.Run(inMemoryExcelSheet, $"{mergedFilePath}{mergedFileName}");

                    break;
                case "2":

                    // 1. Read bank files
                    reader.IsReadFromTheBeginning = false;
                    inMemoryExcelSheet = reader.ReadFiles(bankFilesPath, filePattern);

                    // Remove duplicate rows
                    inMemoryExcelSheet.Truncate();

                    // 2. Read last merged file
                    List<string> filePaths = Directory.GetFiles(mergedFilePath, filePattern).ToList();
                    filePaths.Sort();
                    reader.IsReadFromTheBeginning = true;
                    ExcelSheet lastExcelSheet =  reader.ReadFile(filePaths.Last());

                    // 3. Merge bank files and last merged file
                    inMemoryExcelSheet.MergeWith(lastExcelSheet);

                    // 4. Create merged file
                    writer.Run(inMemoryExcelSheet, $"{mergedFilePath}{mergedFileName}");

                    break;
                case "3":

                    // 1. Clear database
                    transTagConnRepo.DeleteAll();
                    transRepo.DeleteAll();
                    tagRepo.DeleteAll();
                    currencyRepo.DeleteAll();

                    break;
                case "4":
                    // Import merged file to database

                    // 1. Read merged file
                    inMemoryExcelSheet = reader.ReadFile($"{mergedFilePath}{mergedFileName}");

                    // 2. Convert to model
                    converter = new TransactionConverter();
                    var trModels = converter.ConvertToTransactionModel(inMemoryExcelSheet.Transactions);

                    // 3. Save to database
                    transRepo.SmartSave(trModels);

                    break;
                case "5":
                    // Merge merged file with database
                    break;
                case "6":
                    // Export database to file
                    break;
                #endregion

                #region case: Instruction programs
                case "exit":
                    Console.WriteLine("Program was exited.");
                    goto programEnd;
                default:
                    Console.WriteLine("Command not supported!");
                    break;
                #endregion
            }
            goto commandReading;

            programEnd:
            Console.WriteLine("END OF APPLICATION.");
        }
    }
}
