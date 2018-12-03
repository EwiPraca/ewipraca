using System.ComponentModel.DataAnnotations;

namespace EwiPraca.Models
{
    public class UserSettingViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Nazwa")]
        public string SettingDescription { get; set; }
        [Display(Name = "Wartość")]
        [StringLength(1000, ErrorMessage = "Wartość nie może być dłuższa niż 1000 znaków.")]
        public string SettingValue { get; set; }

        public string SettingType { get; set; }
    }
}