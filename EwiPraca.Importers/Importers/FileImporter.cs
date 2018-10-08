using System;
using System.Collections.Generic;
using Excel = Microsoft.Office.Interop.Excel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace EwiPraca.Importers.Importers
{
    public static class FileImporter
    {
        public static List<EmployeeRow> ReadExcelEmployeeFile(string filename)
        {
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(filename);
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;

            int rowCount = xlRange.Rows.Count;
            int colCount = xlRange.Columns.Count;

            List<EmployeeRow> rows = new List<EmployeeRow>();

            for (int i = 1; i <= rowCount; i++)
            {
                EmployeeRow row = new EmployeeRow();
                
                  //  if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)

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

            return null;
        }
    }
}
