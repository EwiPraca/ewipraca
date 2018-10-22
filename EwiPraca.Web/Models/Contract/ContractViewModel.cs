using EwiPraca.Enumerations;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EwiPraca.Models
{
    public class ContractViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public EmployeeViewModel Employee { get; set; }

        [DisplayName("Rodzaj umowy")]
        [Required(ErrorMessage = "Rodzaj umowy jest polem wymaganym.")]
        public ContractType ContractType { get; set; }

        [DisplayName("Początek obowiązywania")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateFrom { get; set; }

        [DisplayName("Data zakończenia obowiązywania")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateTo { get; set; }

        [DisplayName("Miejsce pracy")]
        [Required(ErrorMessage = "Miejsce pracy jest polem wymaganym.")]
        public string Workplace { get; set; }

        [DisplayName("Wynagrodzenie")]
        [Required(ErrorMessage = "Wynagrodzenie jest polem wymaganym.")]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal Salary { get; set; }

        [DisplayName("Rodzaj etatu")]
        [Required(ErrorMessage = "Rodzaj etatu jest polem wymaganym.")]
        public string JobPart { get; set; }

        [DisplayName("Notatki")]
        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }
        public bool IsDeleted { get; set; }
    }
}