using ExcelWorkerApp.Components.ReadExcel;
using ExcelWorkerApp.Components.TruncateTransaction;
using ExcelWorkerApp.Model;
using System;

namespace ExcelWorkerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Read many bank-exported excel files

            // Merge read files

            // => User edits the file

            // Read one merged excel file

            // Load already existing transactions from database

            // Merge db data and merged-excel file data

            // Write to db


            var reader = new ExcelReader<TransactionExtended>();
            var allFiles = reader.Read(@"C:\Users\Aron_Szocs\Documents\Bank", "*.xls");
            //var mergedFile = reader.Read(@"C:\Users\Aron_Szocs\Documents\Bank\Merged\merged.xls");
            //var truncater = new TransactionTruncater<TransactionExtended>();
            //truncater.Run(allFiles);
            reader.TruncateData();

            Console.WriteLine("Hello World!");
        }
    }
}
