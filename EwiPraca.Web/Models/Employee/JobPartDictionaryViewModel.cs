using System.Collections.Generic;

namespace EwiPraca.Models
{
    public class JobPartDictionaryViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public int UserCompanyId { get; set; }
        public List<JobPartDictionaryValueViewModel> Values { get; set; }
    }
}