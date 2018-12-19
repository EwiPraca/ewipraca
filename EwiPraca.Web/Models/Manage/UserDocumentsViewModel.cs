using System.Collections.Generic;

namespace EwiPraca.Models
{
    public class UserDocumentsViewModel
    {
        public List<UserFileViewModel> Files { get; set; }

        public string CurrentFolderGuid { get; set; }
        public string ParentFolderGuid { get; set; }

        public UserDocumentsViewModel()
        {
            if (Files == null)
            {
                Files = new List<UserFileViewModel>();
            }
        }
    }
}