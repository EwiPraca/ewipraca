using EwiPraca.Models.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EwiPraca.Models.Employee
{
    public class EmployeeViewModel : BaseViewModel
    {
        public int Id { get; set; }
        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(100)]
        [Required]
        public string Surname { get; set; }

        [StringLength(200)]
        [Required]
        public string PESEL { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }
        
        public virtual AddressViewModel Address { get; set; }

        public virtual UserCompanyViewModel UserCompany { get; set; }

        public virtual List<ContractViewModel> Contracts { get; set; }
    }
}