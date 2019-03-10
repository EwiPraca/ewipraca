using System.ComponentModel.DataAnnotations;

namespace EwiPraca.Enumerations
{
    public enum LeaveType
    {
        [Display(Name = "Urlop")]
        Vacation = 1,
        [Display(Name = "Zwolnienie lekarskie")]
        SickLeave = 2
    }
}
