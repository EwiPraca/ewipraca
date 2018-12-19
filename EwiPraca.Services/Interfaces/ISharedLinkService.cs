using EwiPraca.Model;

namespace EwiPraca.Services.Interfaces
{
    public interface ISharedLinkService : IService<SharedFileLink>
    {
        SharedFileLink GetByGuid(string guid);
    }
}
