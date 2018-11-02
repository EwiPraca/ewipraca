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
    public class JobPartDictionaryService : IJobPartDictionaryService
    {
        private readonly IRepository<JobPartDictionary> _jobPartDictionaryRepository;
        private readonly IRepository<JobPartDictionaryValue> _jobPartDictionaryValueRepository;
        private readonly IRepository<Employee> _employeeRepository;

        public JobPartDictionaryService(IRepository<JobPartDictionary> jobPartDictionaryRepository,
            IRepository<JobPartDictionaryValue> jobPartDictionaryValueRepository,
            IRepository<Employee> employeeRepository)
        {
            _jobPartDictionaryRepository = jobPartDictionaryRepository;
            _jobPartDictionaryValueRepository = jobPartDictionaryValueRepository;
            _employeeRepository = employeeRepository;
        }

        public IEnumerable<JobPartDictionary> All()
        {
            return _jobPartDictionaryRepository.All().ToList();
        }

        public int Create(JobPartDictionary entity)
        {
            _jobPartDictionaryRepository.Insert(entity);

            return entity.Id;
        }

        public int CreateDictionaryValue(JobPartDictionaryValue entity)
        {
            _jobPartDictionaryValueRepository.Insert(entity);

            return entity.Id;
        }

        public void Delete(JobPartDictionary entity)
        {
            entity.IsDeleted = true;
            _jobPartDictionaryRepository.Update(entity);
        }

        public void DeleteDictionaryValue(JobPartDictionaryValue entity)
        {
            _jobPartDictionaryValueRepository.Delete(entity);
        }

        public JobPartDictionary GetById(int id)
        {
            return _jobPartDictionaryRepository.Query(x => x.Id == id).FirstOrDefault();
        }

        public JobPartDictionary GetByUserCompanyId(int companyId)
        {
            return _jobPartDictionaryRepository.Query(x => x.UserCompanyId == companyId).FirstOrDefault();
        }

        public bool IsPositionInUse(JobPartDictionaryValue value)
        {
            return _employeeRepository.Query(x => !x.IsDeleted && x.PositionDictionaryValueId == value.Id).Count() > 0;
        }

        public void Update(JobPartDictionary entity)
        {
            _jobPartDictionaryRepository.Update(entity);
        }

        public void UpdateDictionaryValue(JobPartDictionaryValue entity)
        {
            _jobPartDictionaryValueRepository.Update(entity);
        }

        public JobPartDictionaryValue GetJobPartValueById(int id)
        {
            return _jobPartDictionaryValueRepository.Query(x => x.Id == id).FirstOrDefault();
        }
    }
}
