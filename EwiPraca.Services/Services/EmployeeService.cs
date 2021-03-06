﻿using EwiPraca.Data.Interfaces;
using EwiPraca.Model;
using EwiPraca.Model.UserArea;
using EwiPraca.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System;

namespace EwiPraca.Services.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<Address> _addressRepository;

        public EmployeeService(IRepository<Employee> employeeRepository,
            IRepository<Address> addressRepository)
        {
            _employeeRepository = employeeRepository;
            _addressRepository = addressRepository;
        }

        public IEnumerable<Employee> All()
        {
            return _employeeRepository.Query(x => !x.IsDeleted).ToList();
        }

        public int Create(Employee entity)
        {
            _employeeRepository.Insert(entity);

            return entity.Id;
        }

        public void Delete(Employee entity)
        {
            entity.IsDeleted = true;
            _employeeRepository.Update(entity);
        }

        public Employee GetById(int id)
        {
            return _employeeRepository.Query(x => x.Id == id).FirstOrDefault();
        }

        public void Update(Employee entity)
        {
            _addressRepository.Update(entity.Address);
            _employeeRepository.Update(entity);
        }

        public Employee GetByPESEL(string PESEL)
        {
            return _employeeRepository.Query(x => x.PESEL == PESEL).FirstOrDefault();
        }

        public List<Employee> GetByCompanyId(int companyId)
        {
            return _employeeRepository.Query(x => x.UserCompanyId == companyId && !x.IsDeleted).ToList();
        }
    }
}