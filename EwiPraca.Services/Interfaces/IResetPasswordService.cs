using EwiPraca.Model;

namespace EwiPraca.Services.Interfaces
{
    public interface IResetPasswordService : IService<ResetPasswordRequest>
    {
        ResetPasswordRequest GetByGuid(string guid);
    }
}
