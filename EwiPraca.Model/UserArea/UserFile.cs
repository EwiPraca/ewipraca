using EwiPraca.Data;
using EwiPraca.Model.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EwiPraca.Model
{
    public class UserFile : BaseModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(260)]
        public string FileName { get; set; }

        [StringLength(100)]
        public string ContentType { get; set; }
        public int? ParentFileId { get; set; }

        [ForeignKey("ParentFileId")]
        public virtual UserFile ParentFile { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserID { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        [Required]
        [StringLength(36)]
        [Index]
        public string FileGuid { get; set; }
    }
}
