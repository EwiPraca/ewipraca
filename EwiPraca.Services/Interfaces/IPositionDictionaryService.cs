using EwiPraca.Model;

namespace EwiPraca.Services.Interfaces
{
    public interface IPositionDictionaryService : IService<PositionDictionary>
    {
        PositionDictionary GetByUserCompanyId(int companyId);
    }
}
