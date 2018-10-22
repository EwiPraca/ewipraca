using System.ComponentModel.DataAnnotations;

namespace EwiPraca.Enumerations
{
    public enum ContractType
    {
        [Display(Name = "Umowa o pracę")]
        UOP,
        [Display(Name = "Umowa o dzieło")]
        ODzielo,
        [Display(Name = "Umowa - zlecenie")]
        Zlecenie,
        [Display(Name = "Umowa - kontrakt")]
        B2B
    }
}