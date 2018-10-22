using EwiPraca.Model.UserArea;
using System.Collections.Generic;

namespace EwiPraca.Services.Interfaces
{
    public interface IUserCompanyService : IService<UserCompany>
    {
        List<UserCompany> GetUserCompanies(string userId);
    }
}