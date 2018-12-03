using System.ComponentModel.DataAnnotations;

namespace EwiPraca.Model
{
    public class Setting
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string SettingName { get; set; }
        [Required]
        [StringLength(200)]
        public string SettingDescription { get; set; }
        [Required]
        [StringLength(50)]
        public string SettingValueType { get; set; }
    }
}
