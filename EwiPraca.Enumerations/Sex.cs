using System.ComponentModel.DataAnnotations;

namespace EwiPraca.Enumerations
{
    public enum Sex
    {
        [Display(Name = "Mężczyzna")]
        Male,
        [Display(Name = "Kobieta")]
        Female
    }
}
