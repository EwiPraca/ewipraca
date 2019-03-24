using EwiPraca.Model;
using System.Collections.Generic;

namespace EwiPraca.Services.Interfaces
{
    public interface IOSHTrainingService : IService<OSHTraining>
    {
        List<OSHTraining> GetOSHTrainingsToExpire(int daysBeforeExpiration);
        List<OSHTraining> GetByCompanyId(int companyId);
    }
}
