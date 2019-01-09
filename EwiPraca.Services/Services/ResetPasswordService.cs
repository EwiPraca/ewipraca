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
        private readonly IRepository<ResetPasswordRequest> _resetPasswordRepository;
        public ResetPasswordService(IRepository<ResetPasswordRequest> resetPasswordRepository)
        {
            _resetPasswordRepository = resetPasswordRepository;
        }
        public IEnumerable<ResetPasswordRequest> All()
        {
            return _resetPasswordRepository.All().ToList();
        }

        public int Create(ResetPasswordRequest entity)
        {
            _resetPasswordRepository.Insert(entity);

            return entity.Id;
        }

        public void Delete(ResetPasswordRequest entity)
        {
            _resetPasswordRepository.Delete(entity);
        }
        
        public ResetPasswordRequest GetByGuid(string guid)
        {
            return _resetPasswordRepository.Query(x => x.ResetGuid == guid).FirstOrDefault();
        }

        public ResetPasswordRequest GetById(int id)
        {
            return _resetPasswordRepository.Query(x => x.Id == id).FirstOrDefault();
        }

        public void Update(ResetPasswordRequest entity)
        {
            _resetPasswordRepository.Update(entity);
        }
    }
}
