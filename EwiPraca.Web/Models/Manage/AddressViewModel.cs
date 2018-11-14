using EwiPraca.Validators;
using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace EwiPraca.Models
{
    [Validator(typeof(UserCompanyAddressValidator))]
    public class AddressViewModel
    {
        public int Id { get; set; }

        [RegularExpression(@"^([a-zA-Z\u0080-\u024F]+(?:. |-| |'))*[a-zA-Z\u0080-\u024F]*$", ErrorMessage = "Niepoprawne znaki w polu Miasto.")]
        [Display(Name = "Miasto")]
        public string City { get; set; }

        [Display(Name = "Ulica")]
        public string StreetName { get; set; }

        [Display(Name = "Numer")]
        public string StreetNumber { get; set; }

        [Display(Name = "Lokal")]
        public string PlaceNumber { get; set; }

        [RegularExpression(@"^[0-9]{2}\-[0-9]{3}$", ErrorMessage = "Niepoprawny format kodu pocztowego.")]
        [Display(Name = "Kod pocztowy w formacie XX-XXX")]
        public string ZIPCode { get; set; }

        public int AddressTypeId { get; set; }
    }
}