using EwiPraca.Model.UserArea;
using System;
using System.Data.Entity;

namespace EwiPraca.Data.Interfaces
{
    public interface IEwiPracaDbContext : IDisposable
    {
        IDbSet<UserCompany> UserCompanies { get; set; }
        IDbSet<Address> UserCompanyAdresses { get; set; }
    }
}
