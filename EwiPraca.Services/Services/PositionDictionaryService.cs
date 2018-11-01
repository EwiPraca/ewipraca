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
        public PositionDictionaryService(IRepository<PositionDictionary> positionDictionaryRepository)
        {
            _positionDictionaryRepository = positionDictionaryRepository;
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

        public void Delete(PositionDictionary entity)
        {
            entity.IsDeleted = true;
            _positionDictionaryRepository.Update(entity);
        }

        public PositionDictionary GetById(int id)
        {
            return _positionDictionaryRepository.Query(x => x.Id == id).FirstOrDefault();
        }

        public PositionDictionary GetByUserCompanyId(int companyId)
        {
            return _positionDictionaryRepository.Query(x => x.UserCompanyId == companyId).FirstOrDefault();
        }

        public void Update(PositionDictionary entity)
        {
            _positionDictionaryRepository.Update(entity);
        }
    }
}
