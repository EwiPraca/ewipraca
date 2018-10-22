using EwiPraca.Model.Base;
using EwiPraca.Model.UserArea;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EwiPraca.Model
{
    public class Employee : BaseModel
    {
        public int Id { get; set; }

        [StringLength(100)]
        [Required]
        public string FirstName { get; set; }

        [StringLength(100)]
        [Required]
        public string Surname { get; set; }

        [StringLength(200)]
        [Required]
        public string PESEL { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public int AddressId { get; set; }
        public virtual Address Address { get; set; }

        [Required]
        public int UserCompanyId { get; set; }
        public virtual UserCompany UserCompany { get; set; }

        public virtual List<Contract> Contracts { get; set; }
    }
}