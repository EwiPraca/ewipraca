using System.ComponentModel.DataAnnotations;

namespace EwiPraca.Model.UserArea
{
    public class UserCompanyAddress
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
    }
}