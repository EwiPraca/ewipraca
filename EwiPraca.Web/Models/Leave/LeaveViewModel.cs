using EwiPraca.Enumerations;
using EwiPraca.Models.Employee;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EwiPraca.Models
{
    public class LeaveViewModel : BaseViewModel
    {
        public int Id { get; set; }

        [DisplayName("Pracownik")]
        [Required(ErrorMessage = "Pracownik jest polem wymaganym.")]
        public int EmployeeId { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Początek - data")]
        [Required(ErrorMessage = "Data początku jest polem wymaganym.")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateFrom { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        [DisplayName("Koniec - data")]
        [Required(ErrorMessage = "Data końca jest polem wymaganym.")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateTo { get; set; } = DateTime.Now;

        [DisplayName("Notatki")]
        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }

        public LeaveType LeaveType { get; set; }

        public List<EwiPracaSelectListItem> Employees { get; set; }

    }
}