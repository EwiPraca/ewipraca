using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static EwiPracaConstants.Validations;

namespace EwiPraca.Models
{
    public class JobPartDictionaryValueViewModel
    {
        public int Id { get; set; }
        public int JobPartDictionaryId { get; set; }
        public int UserCompanyId { get; set; }

        [DisplayName("Nazwa etatu")]
        [Required(ErrorMessage = "Nazwa jest polem wymaganym.")]
        [StringLength(MaximumLength.JobPartNameLength, ErrorMessage = "Maksymalna długość pola to 100 znaków" )]
        public string Name { get; set; }

        [DisplayName("Opis etatu")]
        [StringLength(MaximumLength.DictionaryDescriptionLength, ErrorMessage = "Maksymalna długość pola to 500 znaków")]
        public string Notes { get; set; }

    }
}