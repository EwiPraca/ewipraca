using EwiPraca.Model;

namespace EwiPraca.Services.Interfaces
{
    public interface IJobPartDictionaryService : IService<JobPartDictionary>
    {
        JobPartDictionary GetByUserCompanyId(int companyId);
        int CreateDictionaryValue(JobPartDictionaryValue value);
        void DeleteDictionaryValue(JobPartDictionaryValue value);

        void UpdateDictionaryValue(JobPartDictionaryValue value);
        bool IsPositionInUse(JobPartDictionaryValue value);
        JobPartDictionaryValue GetJobPartValueById(int id);
    }
}
