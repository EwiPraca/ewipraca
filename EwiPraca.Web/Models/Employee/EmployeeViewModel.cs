using EwiPraca.Validators;
using FluentValidation.Attributes;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using EwiPraca.Model;

namespace EwiPraca.Models
{
    [Validator(typeof(EmployeeValidator))]
    public class EmployeeViewModel : BaseViewModel
    {
        public int Id { get; set; }

        [DisplayName("Imię")]
        public string FirstName { get; set; }

        [DisplayName("Nazwisko")]
        public string Surname { get; set; }

        [DisplayName("PESEL")]
        public string PESEL { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Data urodzenia")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; } = DateTime.Now;

        public AddressViewModel Address { get; set; }

        public int UserCompanyId { get; set; }

        [DisplayName("Stanowisko")]
        public int? PositionDictionaryValueId { get; set; }

        [DisplayName("Stanowisko")]
        public PositionDictionaryValue PositionDictionaryValue { get; set; }

        public List<PositionDictionaryValue> Positions { get; set; }

        public List<ContractViewModel> Contracts { get; set; }
        public List<MedicalReportViewModel> MedicalReports { get; set; }
        public List<OSHTrainingViewModel> OSHTrainings { get; set; }
        public List<SickLeaveViewModel> SickLeaves { get; set; }

        public OSHTrainingViewModel LastOSHTraining
        {
            get {
                return OSHTrainings?.OrderByDescending(x => x.NextCompletionDate ?? DateTime.MaxValue).FirstOrDefault();
            }
        }
        public MedicalReportViewModel LastMedicalReport {
            get
            {
                return MedicalReports?.OrderByDescending(x => x.NextCompletionDate ?? DateTime.MaxValue).FirstOrDefault();
            }
        }
    }
}