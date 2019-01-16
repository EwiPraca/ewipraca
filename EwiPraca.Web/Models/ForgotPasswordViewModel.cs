using System.ComponentModel.DataAnnotations;

namespace EwiPraca.Models
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Pole email nie może być puste.")]
        [Display(Name = "Adres email")]
        [EmailAddress(ErrorMessage = "Niepoprawny adres email.")]
        public string Email { get; set; }
    }
}