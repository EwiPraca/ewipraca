using EwiPraca.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EwiPraca.Models
{
    public class MedicalReportViewModel
    {
        public int EmployeeId { get; set; }
        public EmployeeViewModel Employee { get; set; }
        public DateTime CompletionDate { get; set; }
        public DateTime? NextCompletionDate { get; set; }
        public MedicalResultType Result { get; set; }
    }
}