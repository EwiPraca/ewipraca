using EwiPraca.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using EwiPraca.Model;
using EwiPraca.Data.Interfaces;
using EwiPraca.Enumerations;
using System;

namespace EwiPraca.Services.Services
{
    public class EwiFileService : IEwiFileService
    {
        private readonly IRepository<EwiFile> _fileRepository;
        public EwiFileService(IRepository<EwiFile> fileRepository)
        {
            _fileRepository = fileRepository;
        }
        public IEnumerable<EwiFile> All()
        {
            return _fileRepository.Query(x => !x.IsDeleted).ToList();
        }

        public int Create(EwiFile entity)
        {
            _fileRepository.Insert(entity);

            return entity.Id;
        }

        public void Delete(EwiFile entity)
        {
            entity.IsDeleted = true;
            _fileRepository.Update(entity);
        }

        public EwiFile GetById(int id)
        {
            return _fileRepository.Query(x => x.Id == id).FirstOrDefault();
        }

        public string GetEmployeePhoto(int employeeId)
        {
            return _fileRepository.Query(x => x.ParentObjectId == employeeId && x.FileType == FileType.EmployeeProfilePhoto).OrderByDescending(x => x.Id).FirstOrDefault()?.FileName;
        }

        public IEnumerable<EwiFile> GetFilesForEmployee(int employeeId)
        {
            return _fileRepository.Query(x => !x.IsDeleted && x.ParentObjectId == employeeId).AsEnumerable();
        }

        public IEnumerable<EwiFile> GetFilesForEmployeeByFileType(int employeeId, FileType fileType)
        {
            return _fileRepository.Query(x => !x.IsDeleted && x.ParentObjectId == employeeId && x.FileType == fileType).AsEnumerable();
        }

        public void Update(EwiFile entity)
        {
            _fileRepository.Update(entity);
        }
    }
}
