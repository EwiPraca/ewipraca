using EwiPraca.Model;
using System.Collections.Generic;

namespace EwiPraca.Services.Interfaces
{
    public interface IMedicalReportService : IService<MedicalReport>
    {
        List<MedicalReport> GetMedicalReportsToExpire(int daysBeforeExpiration);
    }
}
