using System.ComponentModel.DataAnnotations;

namespace EwiPraca.Model
{
    public class PositionDictionaryValue
    {
        public int Id { get; set; }
        [Required]
        public int PositionDictionaryId { get; set; }
        public virtual PositionDictionary PositionDictionary { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
    }
}
