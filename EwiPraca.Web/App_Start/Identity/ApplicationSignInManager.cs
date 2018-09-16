using EwiPraca.Data;
using Microsoft.AspNet.Identity.Owin;
using System;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace EwiPraca.App_Start.Identity
{
    [ExcludeFromCodeCoverage]
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager,
            IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override async Task<SignInStatus> PasswordSignInAsync(string userName, string password, bool isPersistent, bool shouldLockout)
        {
            var now = DateTime.UtcNow;

            var user = await UserManager.FindByNameAsync(userName);

            if (user == null)
            {
                return SignInStatus.Failure;
            }

            var loginResult = await base.PasswordSignInAsync(userName, password, isPersistent, shouldLockout);

            if (loginResult == SignInStatus.Success)
            {
                user.LastLoginDate = now;
                await UserManager.UpdateAsync(user);
            }

            return loginResult;
        }

    }
}