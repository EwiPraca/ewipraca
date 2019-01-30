using EwiPracaConstants;
using System.ComponentModel.DataAnnotations;

namespace EwiPraca.Models
{
    public class ChangePasswordViewModel
    {
        [Display(Name = "Stare hasło")]
        [Required(ErrorMessage = "Musisz podać stare hasło.")]
        [StringLength(100, ErrorMessage = "{0} musi mieć przynajmniej {2} znaków.", MinimumLength = Validations.MinimalLength.PasswordLength)]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Display(Name = "Nowe hasło")]
        [Required(ErrorMessage = "Musisz podać nowe hasło.")]
        [StringLength(100, ErrorMessage = "{0} musi mieć przynajmniej {2} znaków.", MinimumLength = Validations.MinimalLength.PasswordLength)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Potwierdź hasło")]
        [Required(ErrorMessage = "Musisz potwierdzić hasło.")]
        [Compare("Password", ErrorMessage = "Hasła nie są zgodne.")]
        public string ConfirmPassword { get; set; }
    }
}