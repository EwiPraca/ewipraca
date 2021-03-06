﻿using EwiPraca.Validators;
using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace EwiPraca.Models
{
    [Validator(typeof(UserCompanyValidator))]
    public class UserCompanyViewModel : BaseViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Nazwa firmy")]
        public string CompanyName { get; set; }

        [Display(Name = "Numer REGON")]
        public string REGON { get; set; }

        [Display(Name = "Numer KRS")]
        public string KRS { get; set; }

        [Display(Name = "Numer NIP")]
        public string NIP { get; set; }

        public string ApplicationUserID { get; set; }

        [Display(Name = "Notatki")]
        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }

        public AddressViewModel UserCompanyAddress { get; set; }
    }
}