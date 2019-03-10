using EwiPraca.Model;
using System.Collections.Generic;

namespace EwiPraca.Services.Interfaces
{
    public interface ILeaveService : IService<Leave>
    {
        List<Leave> GetByEmployeeId(int employeeId);
        List<Leave> GetByCompanyId(int companyId);
    }
}
