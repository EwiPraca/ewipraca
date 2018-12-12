using System;

namespace EwiPraca.Models
{
    public class ErrorLogViewModel
    {
        public string FilePath { get; set; }
        public string FileContent { get; set; }
        public string FileName
        {
            get
            {
                string[] path = FilePath.Split('\\');

                return path[path.Length - 1];
            }
        }
    }
}