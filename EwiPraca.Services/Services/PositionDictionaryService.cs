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
    public class PositionDictionaryService : IPositionDictionaryService
    {
        private readonly IRepository<PositionDictionary> _positionDictionaryRepository;
        private readonly IRepository<PositionDictionaryValue> _positionDictionaryValueRepository;
        private readonly IRepository<Employee> _employeeRepository;

        public PositionDictionaryService(IRepository<PositionDictionary> positionDictionaryRepository,
            IRepository<PositionDictionaryValue> positionDictionaryValueRepository,
            IRepository<Employee> employeeRepository)
        {
            _positionDictionaryRepository = positionDictionaryRepository;
            _positionDictionaryValueRepository = positionDictionaryValueRepository;
            _employeeRepository = employeeRepository;
        }

        public IEnumerable<PositionDictionary> All()
        {
            return _positionDictionaryRepository.All().ToList();
        }

        public int Create(PositionDictionary entity)
        {
            _positionDictionaryRepository.Insert(entity);

            return entity.Id;
        }

        public int CreateDictionaryValue(PositionDictionaryValue entity)
        {
            _positionDictionaryValueRepository.Insert(entity);

            return entity.Id;
        }

        public void Delete(PositionDictionary entity)
        {
            entity.IsDeleted = true;
            _positionDictionaryRepository.Update(entity);
        }

        public void DeleteDictionaryValue(PositionDictionaryValue entity)
        {
            _positionDictionaryValueRepository.Delete(entity);
        }

        public PositionDictionary GetById(int id)
        {
            return _positionDictionaryRepository.Query(x => x.Id == id).FirstOrDefault();
        }

        public PositionDictionary GetByUserCompanyId(int companyId)
        {
            return _positionDictionaryRepository.Query(x => x.UserCompanyId == companyId).FirstOrDefault();
        }

        public bool IsPositionInUse(PositionDictionaryValue value)
        {
            return _employeeRepository.Query(x => !x.IsDeleted && x.PositionDictionaryValueId == value.Id).Count() > 0;
        }

        public void Update(PositionDictionary entity)
        {
            _positionDictionaryRepository.Update(entity);
        }

        public void UpdateDictionaryValue(PositionDictionaryValue entity)
        {
            _positionDictionaryValueRepository.Update(entity);
        }

        public PositionDictionaryValue GetPositionValueById(int id)
        {
            return _positionDictionaryValueRepository.Query(x => x.Id == id).FirstOrDefault();
        }
    }
}
