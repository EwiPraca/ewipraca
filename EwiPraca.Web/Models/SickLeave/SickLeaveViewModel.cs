using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EwiPraca.Models
{
    public class SickLeaveViewModel : BaseViewModel
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Początek zwolnienia - data")]
        [Required(ErrorMessage = "Data początku zwolnienia jest polem wymaganym.")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateFrom { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        [DisplayName("Koniec zwolnienia - data")]
        [Required(ErrorMessage = "Data końca zwolnienia jest polem wymaganym.")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateTo { get; set; } = DateTime.Now;

        [DisplayName("Notatki")]
        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }

    }
}