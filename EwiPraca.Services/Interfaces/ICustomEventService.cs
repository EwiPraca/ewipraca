using EwiPraca.Model.EmployeeArea;
using System.Collections.Generic;

namespace EwiPraca.Services.Interfaces
{
    public interface ICustomEventService : IService<CustomEvent>
    {
        List<CustomEvent> GetByCompanyId(int companyId);
    }
}
