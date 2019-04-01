using EwiPraca.Data;
using EwiPraca.Model.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EwiPraca.Model.UserArea
{
    public class UserCompany : BaseModel
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        [Required]
        public string CompanyName { get; set; }

        [StringLength(20)]
        [Required]
        public string REGON { get; set; }

        [StringLength(20)]
        public string KRS { get; set; }

        [StringLength(20)]
        [Required]
        public string NIP { get; set; }

        [StringLength(1000)]
        public string Notes { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserID { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public int AddressId { get; set; }
        public virtual Address Address { get; set; }

        public virtual List<Employee> Employees { get; set; }
        public Guid? CalendarGuid { get; set; }
    }
}