using System.Collections.Generic;

namespace EwiPraca.Models
{
    public class PositionDictionaryViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public int UserCompanyId { get; set; }
        public List<PositionDictionaryValueViewModel> Values { get; set; }
    }
}