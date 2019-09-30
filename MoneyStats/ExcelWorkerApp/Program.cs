using ExcelWorkerApp.Components.ReadExcel;
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

            // => User edits the file

            // Read one merged excel file

            // Load already existing transactions from database

            // Merge db data and merged-excel file data

            // Write to db

            Console.WriteLine("PROGRAM END");
        }
    }
}
