using System.Collections.Generic;

namespace EwiPraca.Models
{
    public class CompanyEmployeesViewModel
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public List<EmployeeViewModel> Employees { get; set; }
    }
}