using System.ComponentModel.DataAnnotations;
using static EwiPracaConstants.Validations;

namespace EwiPraca.Model
{
    public class PositionDictionaryValue
    {
        public int Id { get; set; }
        [Required]
        public int PositionDictionaryId { get; set; }
        public virtual PositionDictionary PositionDictionary { get; set; }

        [Required]
        [StringLength(MaximumLength.PositionNameLength)]
        public string Name { get; set; }
        [StringLength(MaximumLength.DictionaryDescriptionLength)]
        public string Description { get; set; }
    }
}
