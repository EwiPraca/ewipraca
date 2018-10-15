using EwiPraca.Enumerations;
using EwiPraca.Model;
using EwiPraca.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace Ewipraca.ImportProcessors
{
    public class ImportEmployeeProcessor : IImportEmployeeProcessor
    {
        private readonly IEmployeeService _employeeService;
        private readonly IAddressService _addressService;

        public ImportEmployeeProcessor(IEmployeeService employeeService,
            IAddressService addressService)
        {
            _employeeService = employeeService;
            _addressService = addressService;
        }

        public void Process(List<Employee> employeeList, int companyId, bool isOverride)
        {
            var addressType = _addressService.GetAddressTypeByName(AddressType.Zameldowania.ToString());

            foreach (var employee in employeeList)
            {
                var existingEmployee = _employeeService.GetByPESEL(employee.PESEL);
                if(existingEmployee != null)
                {
                    if (isOverride)
                    {
                        existingEmployee.FirstName = employee.FirstName;
                        existingEmployee.Surname = employee.Surname;
                        existingEmployee.BirthDate = employee.BirthDate;
                        existingEmployee.Address.City = employee.Address.City;
                        existingEmployee.Address.StreetName = employee.Address.StreetName;
                        existingEmployee.Address.StreetNumber = employee.Address.StreetNumber;
                        existingEmployee.Address.PlaceNumber = employee.Address.PlaceNumber;
                        existingEmployee.Address.ZIPCode = employee.Address.ZIPCode;

                        existingEmployee.UpdatedDate = DateTime.Now;

                        _employeeService.Update(existingEmployee);
                    }
                }
                else
                {
                    employee.Address.AddressType = addressType;
                    employee.UserCompanyId = companyId;
                    employee.CreatedDate = DateTime.Now;
                    employee.UpdatedDate = employee.CreatedDate;

                    _employeeService.Create(employee);
                }
            }
        }
    }
}
