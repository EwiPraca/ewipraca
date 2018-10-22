using System.ComponentModel.DataAnnotations;
using System.Web;

namespace EwiPraca.Models
{
    public class ImportEmployeesFromExcelViewModel
    {
        public int CompanyId { get; set; }

        [Display(Name = "Nadpisuj istniejące dane")]
        public bool IsOverride { get; set; }

        [Display(Name = "Plik szablonu Excel")]
        [Required(ErrorMessage = "Wybierz plik do importu")]
        public HttpPostedFileBase SpreadsheetFile { get; set; }
    }
}