using System.Collections.Generic;

namespace EwiPraca.Models
{
    public class UserSettingsViewModel
    {
        public string UserId { get; set; }
        public IEnumerable<UserSettingViewModel> Settings { get; set; }
        public bool TwoFactorEnabled { get; set; }
    }
}