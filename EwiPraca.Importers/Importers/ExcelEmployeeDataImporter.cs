using System;
using System.Collections.Generic;
using Excel = Microsoft.Office.Interop.Excel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace EwiPraca.Importers.Importers
{
    public static class ExcelEmployeeDataImporter
    {
        public static List<EmployeeImportRow> ReadExcelEmployeeFile(string filename)
        {
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(filename);
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;

            int rowCount = xlRange.Rows.Count;
            int colCount = xlRange.Columns.Count;

            List<EmployeeImportRow> rows = new List<EmployeeImportRow>();

            for (int i = 2; i <= rowCount; i++)
            {
                EmployeeImportRow row = new EmployeeImportRow();

                row.RowNumber = i;
                row.FirstName = xlRange.Cells[i, 1].Value2?.ToString();
                row.Surname = xlRange.Cells[i, 2].Value2?.ToString();
                row.PESEL = xlRange.Cells[i, 3].Value2?.ToString();
                row.BirthDate = xlRange.Cells[i, 4].Value2?.ToString();
                row.City = xlRange.Cells[i, 5].Value2?.ToString();
                row.StreetName = xlRange.Cells[i, 6].Value2?.ToString();
                row.StreetNumber = xlRange.Cells[i, 7].Value2?.ToString();
                row.PlaceNumber = xlRange.Cells[i, 8].Value2?.ToString();
                row.ZIPCode = xlRange.Cells[i, 9].Value2?.ToString();            

                rows.Add(row);
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();

            Marshal.ReleaseComObject(xlRange);
            Marshal.ReleaseComObject(xlWorksheet);

            //close and release
            xlWorkbook.Close();
            Marshal.ReleaseComObject(xlWorkbook);

            //quit and release
            xlApp.Quit();
            Marshal.ReleaseComObject(xlApp);

            return rows;
        }
    }
}
