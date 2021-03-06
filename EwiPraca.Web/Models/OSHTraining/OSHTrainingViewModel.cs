﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EwiPraca.Models
{
    public class OSHTrainingViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public EmployeeViewModel Employee { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Data szkolenia")]
        [Required(ErrorMessage = "Data szkolenia jest polem wymaganym.")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CompletionDate { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        [DisplayName("Data nastepnęgo szkolenia")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? NextCompletionDate { get; set; }


        [DisplayName("Notatki")]
        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }

        public bool IsValid
        {
            get
            {
                return !NextCompletionDate.HasValue || NextCompletionDate > DateTime.Now;
            }
        }
    }
}