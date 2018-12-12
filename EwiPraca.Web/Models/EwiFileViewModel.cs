using EwiPraca.Enumerations;

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