using EwiPraca.Model;
using System.Collections.Generic;

namespace EwiPraca.Services.Interfaces
{
    public interface ISickLeaveService : IService<SickLeave>
    {
        List<SickLeave> GetByEmployeeId(int employeeId);
    }
}
