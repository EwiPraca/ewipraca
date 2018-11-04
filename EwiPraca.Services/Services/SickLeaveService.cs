using System.Collections.Generic;
using EwiPraca.Model;
using EwiPraca.Services.Interfaces;
using EwiPraca.Data.Interfaces;
using System.Linq;

namespace EwiPraca.Services.Services
{
    public class SickLeaveService : ISickLeaveService
    {
        private readonly IRepository<SickLeave> _sickLeaveRepository;

        public SickLeaveService(IRepository<SickLeave> sickLeaveRepository)
        {
            _sickLeaveRepository = sickLeaveRepository;
        }

        public IEnumerable<SickLeave> All()
        {
            return _sickLeaveRepository.All().ToList();
        }

        public int Create(SickLeave entity)
        {
            _sickLeaveRepository.Insert(entity);

            return entity.Id;
        }

        public void Delete(SickLeave entity)
        {
            entity.IsDeleted = true;
            _sickLeaveRepository.Update(entity);
        }

        public List<SickLeave> GetByEmployeeId(int employeeId)
        {
            return _sickLeaveRepository.Query(x => x.EmployeeId == employeeId && !x.IsDeleted).ToList();
        }

        public SickLeave GetById(int id)
        {
            return _sickLeaveRepository.Query(x => x.Id == id).FirstOrDefault();
        }

        public void Update(SickLeave entity)
        {
            _sickLeaveRepository.Update(entity);
        }
    }
}
