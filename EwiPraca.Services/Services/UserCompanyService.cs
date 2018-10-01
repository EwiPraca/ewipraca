﻿using EwiPraca.Data.Interfaces;
using EwiPraca.Model.UserArea;
using EwiPraca.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace EwiPraca.Services.Services
{
    public class UserCompanyService : IUserCompanyService
    {
        private readonly IRepository<UserCompany> _userCompanyRepository;
        private readonly IRepository<Address> _userCompanyAddressRepository;

        public UserCompanyService(IRepository<UserCompany> userCompanyRepository,
            IRepository<Address> userCompanyAddressRepository)
        {
            _userCompanyRepository = userCompanyRepository;
            _userCompanyAddressRepository = userCompanyAddressRepository;
        }

        public IEnumerable<UserCompany> All()
        {
            return _userCompanyRepository.All().ToList();
        }

        public int Create(UserCompany entity)
        {
            _userCompanyRepository.Insert(entity);

            return entity.Id;
        }

        public int CreateCompanyAddress(Address entity)
        {
            _userCompanyAddressRepository.Insert(entity);

            return entity.Id;
        }

        private void UpdateCompanyAddress(Address entity)
        {
            _userCompanyAddressRepository.Update(entity);
        }

        public void Delete(UserCompany entity)
        {
            _userCompanyRepository.Delete(entity);
        }

        public UserCompany GetById(int id)
        {
            return _userCompanyRepository.Query(x => x.Id == id).FirstOrDefault();
        }

        public void Update(UserCompany entity)
        {
            UpdateCompanyAddress(entity.UserCompanyAddress);
            _userCompanyRepository.Update(entity);
        }

        public List<UserCompany> GetUserCompanies(string userId)
        {
            return _userCompanyRepository.Query(x => x.ApplicationUserID == userId).ToList();
        }
    }
}
