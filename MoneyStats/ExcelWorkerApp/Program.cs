using ExcelWorkerApp.Components.ReadExcel;
using ExcelWorkerApp.Components.WriteExcel;
using ExcelWorkerApp.Model;
using System;

namespace ExcelWorkerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Read many bank-exported excel files
            ExcelReader<Transaction> reader = new ExcelReader<Transaction>();
            ExcelSheet<Transaction> allFiles = reader.Read(@"C:\Users\Aron_Szocs\Documents\Bank", "*.xls");

            // Merge read files
            reader.TruncateData();

            // Write merged file to an excel file
            ExcelWriter excelWriter = new ExcelWriter();
            excelWriter.Run(allFiles, @"C:\Users\Aron_Szocs\Documents\Bank\Merged\Merged.xls");

            // => User edits the file
            Console.WriteLine("======");
            Console.WriteLine("======");
            Console.WriteLine("You should edit the merged file now. Type 'OK' to read the merged file, or 'EXIT' to exit the program.");
            string input = "";
            while (input.ToLower() != "ok" || input.ToLower() == "exit")
            {
                input = Console.ReadLine();
                if (input == "exit")
                {
                    Console.WriteLine("Program exited.");
                    return;
                }
            }

            // Read one merged excel file
            var mergedFileReader = new ExcelReader<TransactionExtended>();
            var mergedFile = mergedFileReader.Read(@"C:\Users\Aron_Szocs\Documents\Bank\Merged\Merged.xls");

            // Load already existing transactions from database

            // Merge db data and merged-excel file data

            // Write to db

            Console.WriteLine("PROGRAM ENDED.");
        }
    }
}
