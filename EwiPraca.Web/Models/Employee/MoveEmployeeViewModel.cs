using EwiPraca.Model.UserArea;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EwiPraca.Models
{
    public class MoveEmployeeViewModel
    {
        public int EmployeeId { get; set; }
        public List<UserCompany> Companies { get; set; }

        [DisplayName("Firma docelowa")]
        [Range(1, int.MaxValue, ErrorMessage = "Proszę wybrać firmę z listy.")]
        public int TargetCompanyId { get; set; }
    }
}