using System.ComponentModel.DataAnnotations;

namespace EwiPraca.Models
{
    public class ExportEmployeesConfirmationViewModel
    {
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Pole hasło nie może być puste.")]
        [Display(Name = "Hasło")]
        public string Password { get; set; }
    }
}