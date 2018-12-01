using EwiPraca.Model;
using System.Collections.Generic;

namespace EwiPraca.Services.Interfaces
{
    public interface IEmployeeService : IService<Employee>
    {
        Employee GetByPESEL(string PESEL);
        List<Employee> GetByCompanyId(int companyId);
    }
}