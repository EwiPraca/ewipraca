﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static EwiPracaConstants.Validations;

namespace EwiPraca.Models
{
    public class PositionDictionaryValueViewModel
    {
        public int Id { get; set; }
        public int PositionDictionaryId { get; set; }
        public int UserCompanyId { get; set; }

        [DisplayName("Nazwa stanowiska")]
        [Required(ErrorMessage = "Nazwa jest polem wymaganym.")]
        [StringLength(MaximumLength.PositionNameLength, ErrorMessage = "Maksymalna długość pola to 150 znaków" )]
        public string Name { get; set; }

        [DisplayName("Opis stanowiska")]
        [StringLength(MaximumLength.DictionaryDescriptionLength, ErrorMessage = "Maksymalna długość pola to 500 znaków")]
        public string Description { get; set; }

    }
}