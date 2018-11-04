using EwiPraca.Model.Base;
using System;

namespace EwiPraca.Model
{
    public class SickLeave : BaseModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string Notes { get; set; }
    }
}
