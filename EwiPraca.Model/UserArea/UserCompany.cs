using EwiPraca.Data;
using EwiPraca.Model.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EwiPraca.Model.UserArea
{
    public class UserCompany : BaseModel
    {
        [Key]
        public int Id { get; set; }
        [StringLength(100)]
        [Required]
        public string CompanyName { get; set; }
        [StringLength(20)]
        [Required]
        public string REGON { get; set; }
        [StringLength(20)]
        public string KRS { get; set; }
        [StringLength(20)]
        [Required]
        public string NIP { get; set; }
        [ForeignKey("ApplicationUser")]
        public string ApplicationUserID { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public int UserCompanyAdressId { get; set; }
        public virtual UserCompanyAddress UserCompanyAdress { get; set; }
    }
}