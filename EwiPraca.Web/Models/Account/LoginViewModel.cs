using System.ComponentModel.DataAnnotations;

namespace EwiPraca.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Pole email nie może być puste.")]
        [Display(Name = "Adres email")]
        [EmailAddress(ErrorMessage = "Niepoprawny adres email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Pole hasło nie może być puste.")]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [Display(Name = "Pamiętaj mnie")]
        public bool RememberMe { get; set; }
    }
}