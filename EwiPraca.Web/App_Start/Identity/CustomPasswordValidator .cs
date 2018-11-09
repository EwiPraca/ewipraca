using Microsoft.AspNet.Identity;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EwiPraca.App_Start.Identity
{
    public class CustomPasswordValidator : IIdentityValidator<string>
    {
        public int RequiredLength { get; set; }
        public CustomPasswordValidator(int length)
        {
            RequiredLength = length;
        }

        public Task<IdentityResult> ValidateAsync(string item)
        {
            if (string.IsNullOrEmpty(item) || item.Length < RequiredLength)
            {
                return Task.FromResult(IdentityResult.Failed(String.Format("Hasło musi mieć przynajmniej {0}", RequiredLength)));
            }

            string pattern = @"^(?=.*[0-9])(?=.*[!@#$%^&*])[0-9a-zA-Z!@#$%^&*0-9]{" + RequiredLength.ToString() + ",}$";

            if (!Regex.IsMatch(item, pattern))
            {
                return Task.FromResult(IdentityResult.Failed("Hasło powinno mieć przynajmniej jedną cyfrę oraz przynajmniej jeden znak specjalny."));
            }

            return Task.FromResult(IdentityResult.Success);
        }
    }
}