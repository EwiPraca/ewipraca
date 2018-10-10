using EwiPraca.Importers.Importers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EwiPraca.Importers
{
    public class CompanyEmployeeImporter : IEwiImporter
    {
        public void Import(string fileName, int companyId)
        {
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException(fileName);
            }

            var results = ExcelEmployeeDataImporter.ReadExcelEmployeeFile(fileName);

            foreach(var result in results)
            {
                if(!result.IsValid())
                {
                    throw new Exception(string.Format("Brakujące dane w wierszu {0}", result.RowNumber));
                }
            }
        }
    }
}
