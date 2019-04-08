using EwiPraca.Enumerations;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EwiPraca.Models.Calendar
{
    public class CustomEventViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayName("Początek - data")]
        [Required(ErrorMessage = "Data początku jest polem wymaganym.")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayName("Koniec - data")]
        [Required(ErrorMessage = "Data końca jest polem wymaganym.")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
        [DisplayName("Tytuł")]
        [Required(ErrorMessage = "Tytuł jest polem wymaganym")]
        public string Title { get; set; }
        [DisplayName("Opis")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Opis jest polem wymaganym.")]
        public string Description { get; set; }

        [DisplayName("Czy wysłać przypomnienie")]
        public bool Reminder { get; set; }

        [DisplayName("Czy zdarzenie jest cykliczne")]
        public bool IsOccurency { get; set; }

        [DisplayName("Typ cykliczności")]
        public OccurencyType? OccurenceType { get; set; }
        [DisplayName("Liczba powtórzeń")]
        public int? OccurencyIntervalNumber { get; set; }
    }
}