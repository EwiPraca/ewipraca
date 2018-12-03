using EwiPraca.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EwiPraca.Model
{
    public class UserSetting
    {
        public int Id { get; set; }
        [Required]
        [ForeignKey("ApplicationUser")]
        public string ApplicationUserID { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        
        public int SettingId { get; set; }
        public virtual Setting Setting { get; set; }
        [Required]
        [StringLength(1000)]
        public string SettingValue { get; set; }
    }
}
