using EwiPraca.Model;
using System.Collections.Generic;
using System.IO;

namespace EwiPraca.Exporters
{
    public interface IEwiExporter
    {
        MemoryStream ExportEmployees(List<Employee> employees, string fileName);
    }
}
