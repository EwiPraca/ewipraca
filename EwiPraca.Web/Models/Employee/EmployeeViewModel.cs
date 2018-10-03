using EwiPraca.Models.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EwiPraca.Models
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
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; } = DateTime.Now;

        public AddressViewModel Address { get; set; }

        public int UserCompanyId { get; set; }
    }
}