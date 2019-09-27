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
            var reader = new ExcelReader<TransactionExtended>();
            var allFiles = reader.Read(@"C:\Users\Aron_Szocs\Documents\Bank", "*.xls");
            //var mergedFile = reader.Read(@"C:\Users\Aron_Szocs\Documents\Bank\Merged\merged.xls");

            var truncater = new TransactionTruncater<TransactionExtended>();
            truncater.Run(allFiles);

            Console.WriteLine("Hello World!");
        }
    }
}
