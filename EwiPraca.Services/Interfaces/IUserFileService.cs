using EwiPraca.Enumerations;
using EwiPraca.Model;
using System.Collections.Generic;

namespace EwiPraca.Services.Interfaces
{
    public interface IUserFileService : IService<UserFile>
    {
        IEnumerable<UserFile> GetRootFilesForUser(string applicationUserId);
        UserFile GetByGuid(string currentFolderGuid);
        List<UserFile> GetUserRootFiles(string userId);
        List<UserFile> GetFilesByFolderGuid(string userId, string folderGuid);
    }
}
