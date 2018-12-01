using EwiPraca.Enumerations;
using EwiPraca.Model.Base;
using System;

namespace EwiPraca.Model
{
    public class MedicalReport : BaseModel, IReminder
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

        public DateTime CompletionDate { get; set; }
        public DateTime? NextCompletionDate { get; set; }
        public MedicalResultType Result { get; set; }

        public string Notes { get; set; }
        public bool ReminderSent { get; set; }
    }
}
