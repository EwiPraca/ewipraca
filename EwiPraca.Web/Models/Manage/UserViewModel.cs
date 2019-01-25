using EwiPraca.Encryptor;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EwiPraca.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Imię")]
        [Required(ErrorMessage = "Musisz podać swoje imię.")]
        [StringLength(100, ErrorMessage = "Pole nie może zawierać więcej niż 100 znaków")]
        public string FirstName { get; set; }

        [Display(Name = "Nazwisko")]
        [Required(ErrorMessage = "Musisz podać swoje nazwisko.")]
        public string Surname { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Adres email jest wymagany.")]
        [EmailAddress(ErrorMessage = "Niepoprawny adres email.")]
        public string Email { get; set; }

        [Display(Name = "Imię")]
        public string FirstNameDecrypted
        {
            get { return EncryptionService.Decrypt(FirstName); }
        }

        [Display(Name = "Nazwisko")]
        public string SurnameDecrypted
        {
            get { return EncryptionService.Decrypt(Surname); }
        }

        [Display(Name = "Email")]
        public string EmailDecrypted
        {
            get { return EncryptionService.DecryptEmail(Email); }
        }

        [Display(Name = "Czy zablokowany")]
        public bool IsActive { get; set; }

        public List<UserCompanyViewModel> UserCompanies { get; set; }
    }
}