using EwiPraca.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using EwiPraca.Model;
using EwiPraca.Data.Interfaces;
using EwiPraca.Enumerations;
using System;

namespace EwiPraca.Services.Services
{
    public class ResetPasswordService : IResetPasswordService
    {
        private readonly IRepository<ResetPasswordRequest> _fileLinksRepository;
        public ResetPasswordService(IRepository<ResetPasswordRequest> fileLinksRepository)
        {
            _fileLinksRepository = fileLinksRepository;
        }
        public IEnumerable<ResetPasswordRequest> All()
        {
            return _fileLinksRepository.All().ToList();
        }

        public int Create(ResetPasswordRequest entity)
        {
            _fileLinksRepository.Insert(entity);

            return entity.Id;
        }

        public void Delete(ResetPasswordRequest entity)
        {
            _fileLinksRepository.Delete(entity);
        }
        
        public ResetPasswordRequest GetByGuid(string guid)
        {
            return _fileLinksRepository.Query(x => x.ResetGuid == guid).FirstOrDefault();
        }

        public ResetPasswordRequest GetById(int id)
        {
            return _fileLinksRepository.Query(x => x.Id == id).FirstOrDefault();
        }

        public void Update(ResetPasswordRequest entity)
        {
            _fileLinksRepository.Update(entity);
        }
    }
}
