using EwiPraca.Model;
using System.Collections.Generic;

namespace Ewipraca.ImportProcessors
{
    public interface IImportEmployeeProcessor
    {
        void Process(List<Employee> employeeList, int companyId, bool isOverride);
    }
}