using EwiPraca.Enumerations;
using EwiPraca.Model.Base;
using System;

namespace EwiPraca.Model
{
    public class Leave : BaseModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
        public LeaveType LeaveType { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string Notes { get; set; }
    }
}
