﻿using EwiPraca.Validators;
using FluentValidation.Attributes;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EwiPraca.Models
{
    [Validator(typeof(EmployeeValidator))]
    public class EmployeeViewModel : BaseViewModel
    {
        private List<ContractViewModel> _contracts;
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

        public List<ContractViewModel> Contracts { get; set; }
        public List<MedicalReportViewModel> MedicalReports { get; set; }

        public List<ContractViewModel> ContractsList
        {
            get
            {
                return _contracts.Where(x => !x.IsDeleted).ToList();
            }
            set
            {
                _contracts = value;
            }
        }
    }
}