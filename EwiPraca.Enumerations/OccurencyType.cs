using System.ComponentModel.DataAnnotations;

namespace EwiPraca.Enumerations
{
    public enum OccurencyType
    {
        [Display(Name = "Codziennie")]
        Daily = 1,
        [Display(Name = "Co tydzień")]
        Weekly = 7,
    }
}
