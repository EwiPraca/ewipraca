using EwiPracaConstants;
using System.ComponentModel.DataAnnotations;

namespace EwiPraca.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Musisz podać swoje imię.")]
        [Display(Name = "Imię")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Musisz podać swoje nazwisko.")]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Adres email jest wymagany.")]
        [EmailAddress(ErrorMessage = "Niepoprawny adres email.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Hasło")]
        [Required(ErrorMessage = "Musisz podać hasło.")]
        [StringLength(100, ErrorMessage = "{0} musi mieć przynajmniej {2} znaków.", MinimumLength = Validations.MinimalLength.PasswordLength)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Potwierdź hasło")]
        [Required(ErrorMessage = "Musisz potwierdzić hasło.")]
        [Compare("Password", ErrorMessage = "Hasła nie są zgodne.")]
        public string ConfirmPassword { get; set; }
    }
}