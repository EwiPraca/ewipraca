using System.ComponentModel.DataAnnotations;

namespace EwiPraca.Model.UserArea
{
    public class AddressType
    {
        [Key]
        public int Id { get; set; }

        [StringLength(30)]
        [Required]
        public string AddressTypeName { get; set; }

        [StringLength(100)]
        [Required]
        public string AddressTypeNameDescription { get; set; }
    }
}
