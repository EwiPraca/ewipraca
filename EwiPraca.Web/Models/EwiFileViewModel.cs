using EwiPraca.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EwiPraca.Models
{
    public class EwiFileViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public FileType FileType { get; set; }
        public int ParentObjectId { get; set; }

        public string DisplayedFileName
        {
            get
            {
                string[] path = FileName.Split('/');

                return path[path.Length - 1];
            }
        }
    }
}