using System.ComponentModel.DataAnnotations;

namespace EwiPraca.Models
{
    public class UserCompanyAddressViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Miasto")]
        public string City { get; set; }
        [Required]
        [Display(Name = "Ulica")]
        [StringLength(100)]
        public string StreetName { get; set; }
        [Required]
        [Display(Name = "Numer")]
        [StringLength(10)]
        public string StreetNumber { get; set; }
        [Display(Name = "Lokal")]
        [StringLength(10)]
        public string PlaceNumber { get; set; }
        [Required]
        [Display(Name = "Kod pocztowy")]
        [StringLength(10)]
        public string ZIPCode { get; set; }
    }
}