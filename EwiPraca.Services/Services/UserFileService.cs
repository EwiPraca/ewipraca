using EwiPraca.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using EwiPraca.Model;
using EwiPraca.Data.Interfaces;
using EwiPraca.Enumerations;
using System;

namespace EwiPraca.Services.Services
{
    public class UserFileService : IUserFileService
    {
        private readonly IRepository<UserFile> _fileRepository;
        public UserFileService(IRepository<UserFile> fileRepository)
        {
            _fileRepository = fileRepository;
        }
        public IEnumerable<UserFile> All()
        {
            return _fileRepository.Query(x => !x.IsDeleted).ToList();
        }

        public int Create(UserFile entity)
        {
            _fileRepository.Insert(entity);

            return entity.Id;
        }

        public void Delete(UserFile entity)
        {
            entity.IsDeleted = true;
            _fileRepository.Update(entity);
        }

        public List<UserFile> GetUserRootFiles(string userId)
        {
            return _fileRepository.Query(x => x.ApplicationUserID == userId).ToList();
        }

        public UserFile GetByGuid(string fileGuid)
        {
            return _fileRepository.Query(x => x.FileGuid == fileGuid).FirstOrDefault();
        }

        public UserFile GetById(int id)
        {
            return _fileRepository.Query(x => x.Id == id).FirstOrDefault();
        }

        public IEnumerable<UserFile> GetRootFilesForUser(string applicationUserId)
        {
            return _fileRepository.Query(x => x.ApplicationUserID == applicationUserId && !x.ParentFileId.HasValue && !x.IsDeleted).ToList();
        }

        public void Update(UserFile entity)
        {
            _fileRepository.Update(entity);
        }

        public List<UserFile> GetFilesByFolderGuid(string userId, string folderGuid)
        {
            var folder = GetByGuid(folderGuid);
            return _fileRepository.Query(x => x.ApplicationUserID == userId && x.ParentFileId.HasValue && x.ParentFileId == folder.Id && !x.IsDeleted).ToList();
        }
    }
}
