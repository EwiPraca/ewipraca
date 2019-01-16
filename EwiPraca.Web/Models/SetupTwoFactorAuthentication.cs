using System.ComponentModel;

namespace EwiPraca.Models
{
    public class SetupTwoFactorAuthentication
    {
        [DisplayName("Manualny kod dostępu")]
        public string ManualEntryKey { get; set; }

        [DisplayName("Zeskanuj kod w aplikacji")]
        public string QrCodeSetupImageUrl { get; set; }
    }
}