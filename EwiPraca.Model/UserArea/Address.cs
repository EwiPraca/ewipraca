using System.ComponentModel.DataAnnotations;

namespace EwiPraca.Model.UserArea
{
    public class Address
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string City { get; set; }

        [Required]
        [StringLength(100)]
        public string StreetName { get; set; }

        [Required]
        [StringLength(10)]
        public string StreetNumber { get; set; }

        [StringLength(10)]
        public string PlaceNumber { get; set; }

        [Required]
        [StringLength(10)]
        public string ZIPCode { get; set; }

        public int AddressTypeId { get; set; }
        public virtual AddressType AddressType { get; set; }
    }
}