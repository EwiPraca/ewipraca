using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EwiPraca.Models
{
    public class UserFileViewModel
    {
        public string ParentFolderName { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public string DisplayedFileName
        {
            get
            {
                string[] path = FileName.Split('/');

                return path[path.Length - 1];
            }
        }

        public string FolderGuid { get; set; }
        public string FileGuid { get; set; }
    }
}