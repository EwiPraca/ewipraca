using System.ComponentModel.DataAnnotations;
using static EwiPracaConstants.Validations;

namespace EwiPraca.Model
{
    public class JobPartDictionaryValue
    {
        public int Id { get; set; }
        [Required]
        public int JobPartDictionaryId { get; set; }
        public virtual JobPartDictionary JobPartDictionary { get; set; }

        [Required]
        [StringLength(MaximumLength.JobPartNameLength)]
        public string Name { get; set; }
        [StringLength(MaximumLength.DictionaryDescriptionLength)]
        public string Notes { get; set; }
    }
}
