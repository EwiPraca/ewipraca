using EwiPraca.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EwiPraca.Model;
using EwiPraca.Data.Interfaces;

namespace EwiPraca.Services.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepository<Employee> _employeeRepository;

        public EmployeeService(IRepository<Employee> employeeService)
        {
            _employeeRepository = employeeService;
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
            throw new NotImplementedException();
        }
    }
}
