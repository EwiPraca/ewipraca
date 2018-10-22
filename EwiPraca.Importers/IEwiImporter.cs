using EwiPraca.Importers.Importers;
using System.Collections.Generic;

namespace EwiPraca.Importers
{
    public interface IEwiImporter
    {
        List<EmployeeImportRow> Import(string fileName, int companyId);
    }
}