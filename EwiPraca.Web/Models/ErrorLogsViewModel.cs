using System.Collections.Generic;

namespace EwiPraca.Models
{
    public class ErrorLogsViewModel
    {
        public List<ErrorLogViewModel> ErrorLogs { get; set; }

        public ErrorLogsViewModel()
        {
            if(ErrorLogs == null)
            {
                ErrorLogs = new List<ErrorLogViewModel>();
            }
        }
    }
}