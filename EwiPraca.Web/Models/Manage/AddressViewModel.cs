using EwiPraca.Validators;
using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace EwiPraca.Models
{
    [Validator(typeof(UserCompanyAddressValidator))]
    public class AddressViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Miasto")]
        public string City { get; set; }

        [Display(Name = "Ulica")]
        public string StreetName { get; set; }

        [Display(Name = "Numer")]
        public string StreetNumber { get; set; }

        [Display(Name = "Lokal")]
        public string PlaceNumber { get; set; }

        [Display(Name = "Kod pocztowy")]
        public string ZIPCode { get; set; }

        public int AddressTypeId { get; set; }
    }
}