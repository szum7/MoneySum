using ExcelWorkerApp.Model;
using ExcelWorkerAppV2.Utility;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.Model;
using NPOI.HSSF.UserModel;
using System.Collections.Generic;
using System.IO;

namespace ExcelWorkerAppV2
{
    /// <summary>
    /// Create a file from memory
    /// </summary>
    class ExcelWriter
    {
        ConsoleWatch watch;
        const string NAME = "ExcelWriter";

        public ExcelWriter()
        {
            this.watch = new ConsoleWatch(NAME);
        }

        public void Run(ExcelSheet excelData, string fullFilePath, bool isCreateExtendedHeader = true)
        {
            this.watch.StartAll();
            this.watch.PrintTime($"Started writing to excel.");

            using (var fs = new FileStream(fullFilePath, FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                if (Path.GetExtension(fullFilePath) == ".xlsx")
                {
                    workbook = new XSSFWorkbook();
                }
                else
                {
                    workbook = HSSFWorkbook.Create(InternalWorkbook.CreateWorkbook());
                }
                this.watch.PrintDiff($"File created.");

                ISheet sheet = workbook.CreateSheet("Könyvelt tételek");

                this.CreateHeader(sheet, excelData.Header, isCreateExtendedHeader);

                IDataFormat newDataFormat = workbook.CreateDataFormat();
                ICellStyle dateStyle = workbook.CreateCellStyle();
                dateStyle.DataFormat = newDataFormat.GetFormat("yyyy.MM.dd");

                ICellStyle numberStyle = workbook.CreateCellStyle();
                numberStyle.DataFormat = newDataFormat.GetFormat("0.00");

                int rowIndex = 1;
                foreach (var item in excelData.Transactions)
                {
                    IRow row = sheet.CreateRow(rowIndex);

                    var dateCell = this.CreateStyledCell(row, 0, dateStyle);
                    var numberCell = this.CreateStyledCell(row, 7, numberStyle);

                    dateCell.SetCellValue(item.AccountingDate);
                    row.CreateCell(1).SetCellValue(item.TransactionId);
                    row.CreateCell(2).SetCellValue(item.Type);
                    row.CreateCell(3).SetCellValue(item.Account);
                    row.CreateCell(4).SetCellValue(item.AccountName);
                    row.CreateCell(5).SetCellValue(item.PartnerAccount);
                    row.CreateCell(6).SetCellValue(item.PartnerName);
                    numberCell.SetCellValue(item.Sum);
                    row.CreateCell(8).SetCellValue(item.Currency);
                    row.CreateCell(9).SetCellValue(item.Message);

                    row.CreateCell(10).SetCellValue(item.IsOmitted ? "1" : "");
                    row.CreateCell(11).SetCellValue(item.GroupId);
                    row.CreateCell(12).SetCellValue(string.Join(",", item.TagNames));
                    row.CreateCell(13).SetCellValue(item.TagGroupId);

                    this.watch.PrintDiff($"{rowIndex}/{excelData.TransactionCount} line{(rowIndex == 1 ? "" : "s")} created.");
                    rowIndex++;
                }

                workbook.Write(fs);
                this.watch.PrintDiff($"File saved.");
            }
            this.watch.PrintTime($"Finished writing the excel file.\n");
            this.watch.StopAll();
        }

        #region Run utilities
        ICell CreateStyledCell(IRow row, int columnIndex, ICellStyle style)
        {
            ICell cell = row.CreateCell(columnIndex);
            cell.CellStyle = style;
            return cell;
        }

        void CreateHeader(ISheet sheet, List<string> header, bool isCreateExtendedHeader)
        {
            IRow row = sheet.CreateRow(0);
            for (int i = 0; i < header.Count; i++)
            {
                row.CreateCell(i).SetCellValue(header[i]);
            }
            if (isCreateExtendedHeader)
            {
                row.CreateCell(10).SetCellValue("Is omitted?");
                row.CreateCell(11).SetCellValue("Group id");
                row.CreateCell(12).SetCellValue("Tag names");
                row.CreateCell(13).SetCellValue("Tag group id");
            }
        }
        #endregion
    }
}
