using EwiPracaConstants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EwiPraca.Models
{
    public class ResetPasswordViewModel
    {
        [Display(Name = "Hasło")]
        [Required(ErrorMessage = "Musisz podać hasło.")]
        [StringLength(100, ErrorMessage = "{0} musi mieć przynajmniej {2} znaków.", MinimumLength = Validations.MinimalLength.PasswordLength)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Potwierdź hasło")]
        [Required(ErrorMessage = "Musisz potwierdzić hasło.")]
        [Compare("Password", ErrorMessage = "Hasła nie są zgodne.")]
        public string ConfirmPassword { get; set; }

        public string Email { get; set; }
        public string Guid { get; set; }
    }
}