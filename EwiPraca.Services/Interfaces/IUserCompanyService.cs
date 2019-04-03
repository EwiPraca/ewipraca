using EwiPraca.Model.UserArea;
using System;
using System.Collections.Generic;

namespace EwiPraca.Services.Interfaces
{
    public interface IUserCompanyService : IService<UserCompany>
    {
        List<UserCompany> GetUserCompanies(string userId);
        UserCompany GetByGuid(Guid guid);
    }
}