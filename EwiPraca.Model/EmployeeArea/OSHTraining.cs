using EwiPraca.Model.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace EwiPraca.Model
{
    public class OSHTraining : BaseModel//, IReminder
    {
        [Key]
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

        public DateTime CompletionDate { get; set; }
        public DateTime? NextCompletionDate { get; set; }

        public string Notes { get; set; }
        public bool ReminderSent { get; set; }
    }
}