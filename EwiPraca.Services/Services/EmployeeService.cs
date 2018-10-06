using EwiPraca.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EwiPraca.Model;
using EwiPraca.Data.Interfaces;
using EwiPraca.Model.UserArea;

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
    }
}
