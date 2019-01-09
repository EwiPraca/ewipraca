using System;
using System.Web.Configuration;

namespace EwiPraca
{
    public static class SettingsHandler
    {
        public static int DaysBeforeIntervalReminder { get; set; } = Convert.ToInt32(WebConfigurationManager.AppSettings["NumberOfDaysBeforeReminder"] ?? "0");
        public static int PasswordResetEmailValidDuration { get; set; } = Convert.ToInt32(WebConfigurationManager.AppSettings["PasswordResetEmailValidDuration"] ?? "1");        
    }
}