using System;
using System.Text;

namespace EwiPraca.Encryptor
{
    public static class EncryptionService
    {
        public static string Encrypt(string text)
        {
            var bytes = Encoding.Unicode.GetBytes(text);

            return Convert.ToBase64String(bytes);
        }

        public static string Decrypt(string text)
        {
            var bytes = Convert.FromBase64String(text);

            return Encoding.Unicode.GetString(bytes);
        }

        public static string EncryptEmail(string email)
        {
            var splittedString = email.Split('@');

            splittedString[0] = Encrypt(splittedString[0]);
            return splittedString[0] + "@" + splittedString[1];
        }

        public static string DecryptEmail(string email)
        {
            var splittedString = email.Split('@');

            splittedString[0] = Decrypt(splittedString[0]);
            return splittedString[0] + "@" + splittedString[1];
        }
    }
}