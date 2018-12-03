using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EwiPraca.Model;

namespace EwiPraca.Models
{
    public class UserSettingsViewModel
    {
        public string UserId { get; set; }
        public IEnumerable<UserSettingViewModel> Settings { get; set; }
    }
}