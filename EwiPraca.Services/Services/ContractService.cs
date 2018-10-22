using EwiPraca.Data.Interfaces;
using EwiPraca.Model;
using EwiPraca.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EwiPraca.Services.Services
{
    public class ContractService : IContractService
    {
        private readonly IRepository<Contract> _contractRepository;

        public ContractService(IRepository<Contract> contractRepository)
        {
            _contractRepository = contractRepository;
        }

        public IEnumerable<Contract> All()
        {
            return _contractRepository.All().ToList();
        }

        public int Create(Contract entity)
        {
            _contractRepository.Insert(entity);

            return entity.Id;
        }

        public void Delete(Contract entity)
        {
            entity.IsDeleted = true;
            _contractRepository.Update(entity);
        }

        public List<Contract> GetByEmployeeId(int employeeId)
        {
            return _contractRepository.Query(x => x.EmployeeId == employeeId && !x.IsDeleted).ToList();
        }

        public Contract GetById(int id)
        {
            return _contractRepository.Query(x => x.Id == id).FirstOrDefault();
        }

        public void Update(Contract entity)
        {
            _contractRepository.Update(entity);
        }
    }
}