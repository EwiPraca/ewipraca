using EwiPraca.Enumerations;
using EwiPraca.Model.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace EwiPraca.Model
{
    public class Contract : BaseModel
    {
        public int Id { get; set; }

        public virtual Employee Employee { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        [Required]
        public ContractType ContractType { get; set; }

        public string Workplace { get; set; }

        [Required]
        public decimal Salary { get; set; }

        public string JobPart { get; set; }
        public string Notes { get; set; }
    }
}