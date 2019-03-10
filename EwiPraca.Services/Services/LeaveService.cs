using System.Collections.Generic;
using EwiPraca.Model;
using EwiPraca.Services.Interfaces;
using EwiPraca.Data.Interfaces;
using System.Linq;
using System;

namespace EwiPraca.Services.Services
{
    public class LeaveService : ILeaveService
    {
        private readonly IRepository<Leave> _leaveRepository;

        public LeaveService(IRepository<Leave> leaveRepository)
        {
            _leaveRepository = leaveRepository;
        }

        public IEnumerable<Leave> All()
        {
            return _leaveRepository.All().ToList();
        }

        public int Create(Leave entity)
        {
            _leaveRepository.Insert(entity);

            return entity.Id;
        }

        public void Delete(Leave entity)
        {
            entity.IsDeleted = true;
            _leaveRepository.Update(entity);
        }

        public List<Leave> GetByCompanyId(int companyId)
        {
            return _leaveRepository.Query(x => x.Employee.UserCompanyId == companyId && !x.IsDeleted).ToList();
        }

        public List<Leave> GetByEmployeeId(int employeeId)
        {
            return _leaveRepository.Query(x => x.EmployeeId == employeeId && !x.IsDeleted).ToList();
        }

        public Leave GetById(int id)
        {
            return _leaveRepository.Query(x => x.Id == id).FirstOrDefault();
        }

        public void Update(Leave entity)
        {
            _leaveRepository.Update(entity);
        }
    }
}
