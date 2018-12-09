using EwiPraca.Enumerations;
using EwiPraca.Model;
using System.Collections.Generic;

namespace EwiPraca.Services.Interfaces
{
    public interface IEwiFileService : IService<EwiFile>
    {
        IEnumerable<EwiFile> GetFilesForEmployeeByFileType(int employeeId, FileType fileType);
        IEnumerable<EwiFile> GetFilesForEmployee(int employeeId);
    }
}
