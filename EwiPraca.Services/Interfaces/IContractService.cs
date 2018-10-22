using EwiPraca.Model;
using System.Collections.Generic;

namespace EwiPraca.Services.Interfaces
{
    public interface IContractService : IService<Contract>
    {
        List<Contract> GetByEmployeeId(int employeeId);
    }
}