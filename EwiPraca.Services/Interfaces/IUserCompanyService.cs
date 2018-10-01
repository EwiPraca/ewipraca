using EwiPraca.Model.UserArea;
using System.Collections.Generic;

namespace EwiPraca.Services.Interfaces
{
    public interface IUserCompanyService : IService<UserCompany>
    {
        int CreateCompanyAddress(Address entity);
        List<UserCompany> GetUserCompanies(string userId);
    }
}
