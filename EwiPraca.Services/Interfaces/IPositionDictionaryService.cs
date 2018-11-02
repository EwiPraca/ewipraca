using EwiPraca.Model;

namespace EwiPraca.Services.Interfaces
{
    public interface IPositionDictionaryService : IService<PositionDictionary>
    {
        PositionDictionary GetByUserCompanyId(int companyId);
        int CreateDictionaryValue(PositionDictionaryValue value);
        void DeleteDictionaryValue(PositionDictionaryValue value);

        void UpdateDictionaryValue(PositionDictionaryValue value);
        bool IsPositionInUse(PositionDictionaryValue value);
        PositionDictionaryValue GetPositionValueById(int id);
    }
}
