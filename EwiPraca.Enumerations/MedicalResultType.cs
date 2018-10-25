using System.ComponentModel.DataAnnotations;

namespace EwiPraca.Enumerations
{
    public enum MedicalResultType
    {
        [Display(Name = "Pozytywny")]
        Positive,
        [Display(Name = "Negatywny")]
        Negative
    }
}
