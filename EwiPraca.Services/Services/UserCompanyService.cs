using EwiPraca.Data.Interfaces;
using EwiPraca.Model.UserArea;
using EwiPraca.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System;

namespace EwiPraca.Services.Services
{
    public class UserCompanyService : IUserCompanyService
    {
        private readonly IRepository<UserCompany> _userCompanyRepository;
        private readonly IAddressService _addressService;

        public UserCompanyService(IRepository<UserCompany> userCompanyRepository,
            IAddressService addressService)
        {
            _userCompanyRepository = userCompanyRepository;
            _addressService = addressService;
        }

        public IEnumerable<UserCompany> All()
        {
            return _userCompanyRepository.Query(x => !x.IsDeleted).ToList();
        }

        public int Create(UserCompany entity)
        {
            _userCompanyRepository.Insert(entity);

            return entity.Id;
        }

        public void Delete(UserCompany entity)
        {
            entity.IsDeleted = true;
            _userCompanyRepository.Update(entity);
        }

        public UserCompany GetById(int id)
        {
            return _userCompanyRepository.Query(x => x.Id == id).FirstOrDefault();
        }

        public void Update(UserCompany entity)
        {
            _addressService.Update(entity.Address);
            _userCompanyRepository.Update(entity);
        }

        public List<UserCompany> GetUserCompanies(string userId)
        {
            return _userCompanyRepository.Query(x => x.ApplicationUserID == userId).ToList();
        }

        public UserCompany GetByGuid(Guid guid)
        {
            return _userCompanyRepository.Query(x => x.CalendarGuid == guid).FirstOrDefault();
        }
    }
}