using EwiPraca.Validators;
using FluentValidation.Attributes;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using EwiPraca.Model;
using EwiPraca.Enumerations;
using System.Web.Mvc;

namespace EwiPraca.Models
{
    [Validator(typeof(EmployeeValidator))]
    public class EmployeeViewModel : BaseViewModel
    {
        public SelectList Gender { get; set; }

        public int Id { get; set; }

        [DisplayName("Imię")]
        [RegularExpression(@"^[\p{L} \.'\-]+$", ErrorMessage = "Niepoprawne znaki w polu Imię.")]
        public string FirstName { get; set; }

        [DisplayName("Nazwisko")]
        [RegularExpression(@"^[\p{L} \.'\-]+$", ErrorMessage = "Niepoprawne znaki w polu Nazwisko.")]
        public string Surname { get; set; }

        [DisplayName("PESEL")]
        public string PESEL { get; set; }

        [DisplayName("Zdjęcie")]
        public string PhotoURL { get; set; }

        [DisplayName("Płeć")]
        public Sex Sex { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Data urodzenia")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; } = DateTime.Now;

        public AddressViewModel Address { get; set; }

        public int UserCompanyId { get; set; }

        [DisplayName("Stanowisko")]
        public int? PositionDictionaryValueId { get; set; }

        [DisplayName("Stanowisko")]
        public PositionDictionaryValue Position { get; set; }

        public List<PositionDictionaryValue> Positions { get; set; }

        public List<ContractViewModel> Contracts { get; set; }
        public List<EwiFileViewModel> Files { get; set; }
        public List<MedicalReportViewModel> MedicalReports { get; set; }
        public List<OSHTrainingViewModel> OSHTrainings { get; set; }
        public List<LeaveViewModel> Leaves { get; set; }

        public OSHTrainingViewModel LastOSHTraining
        {
            get
            {
                return OSHTrainings?.OrderByDescending(x => x.NextCompletionDate ?? DateTime.MaxValue).FirstOrDefault();
            }
        }
        public MedicalReportViewModel LastMedicalReport
        {
            get
            {
                return MedicalReports?.OrderByDescending(x => x.NextCompletionDate ?? DateTime.MaxValue).FirstOrDefault();
            }
        }
    }
}