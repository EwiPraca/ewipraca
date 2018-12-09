using EwiPraca.Enumerations;
using EwiPraca.Model.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EwiPraca.Model
{
    public class EwiFile : BaseModel
    {
        public int Id { get;  set; }

        [Required]
        [StringLength(260)]
        public string FileName { get; set; }

        [Required]
        [StringLength(100)]
        public string ContentType { get; set; }
        public FileType FileType { get; set; }

        [Index]
        public int ParentObjectId { get; set; }
    }
}
