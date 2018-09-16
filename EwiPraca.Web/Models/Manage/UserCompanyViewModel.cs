using EwiPraca.Validators;
using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace EwiPraca.Models
{
    [Validator(typeof(UserCompanyValidator))]
    public class UserCompanyViewModel
    {
        public int Id { get; set; }
        //[Required(ErrorMessage = "Pole Nazwa Firmy nie może być puste.")]
        [Display(Name = "Nazwa firmy")]
        public string CompanyName { get; set; }
        
        [Display(Name = "Numer REGON")]
        public string REGON { get; set; }

        [StringLength(20, ErrorMessage = "Pole nie może być dłuższe niż 20 znaków.")]
        [Display(Name = "Numer KRS")]
        public string KRS { get; set; }
        
        [Display(Name = "Numer NIP")]
        public string NIP { get; set; }

        public UserCompanyAddressViewModel UserCompanyAddress {get;set;}
    }
}