using EwiPraca.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using EwiPraca.Model;
using EwiPraca.Data.Interfaces;
using EwiPraca.Enumerations;
using System;

namespace EwiPraca.Services.Services
{
    public class SharedLinkService : ISharedLinkService
    {
        private readonly IRepository<SharedFileLink> _fileLinksRepository;
        public SharedLinkService(IRepository<SharedFileLink> fileLinksRepository)
        {
            _fileLinksRepository = fileLinksRepository;
        }
        public IEnumerable<SharedFileLink> All()
        {
            return _fileLinksRepository.All().ToList();
        }

        public int Create(SharedFileLink entity)
        {
            _fileLinksRepository.Insert(entity);

            return entity.Id;
        }

        public void Delete(SharedFileLink entity)
        {
            _fileLinksRepository.Delete(entity);
        }
        
        public SharedFileLink GetByGuid(string fileGuid)
        {
            return _fileLinksRepository.Query(x => x.GuidLink == fileGuid).FirstOrDefault();
        }

        public SharedFileLink GetById(int id)
        {
            return _fileLinksRepository.Query(x => x.Id == id).FirstOrDefault();
        }

        public void Update(SharedFileLink entity)
        {
            _fileLinksRepository.Update(entity);
        }
    }
}
