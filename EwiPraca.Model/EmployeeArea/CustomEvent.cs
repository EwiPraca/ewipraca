using EwiPraca.Enumerations;
using EwiPraca.Model.Base;
using EwiPraca.Model.UserArea;
using System;
using System.ComponentModel.DataAnnotations;

namespace EwiPraca.Model.EmployeeArea
{
    public class CustomEvent : BaseModel
    {
        [Key]
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public virtual UserCompany Company { get; set; }
        [Required]
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [Required]
        public string Description { get; set; }

        public bool IsOccurency { get; set; }
        public int? OccurencyIntervalNumber { get; set; }

        public OccurencyType? OccurenceType { get; set; }
        public bool Reminder { get; set; }
    }
}
