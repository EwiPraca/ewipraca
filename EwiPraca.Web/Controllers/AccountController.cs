using EwiPraca.App_Start.Identity;
using EwiPraca.Data;
using EwiPraca.Encryptor;
using EwiPraca.Model;
using EwiPraca.Models;
using EwiPraca.Services.Interfaces;
using Google.Authenticator;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using NLog;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EwiPraca.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly ApplicationUserManager _userManager;
        private readonly ApplicationSignInManager _signInManager;
        private readonly IResetPasswordService _resetPasswordService;
        private readonly IEmailMessageService _emailMessageService;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public AccountController(
            ApplicationUserManager userManager,
            ApplicationSignInManager signInManager,
            IResetPasswordService resetPasswordService,
            IEmailMessageService emailMessageService
            )
        {
            _resetPasswordService = resetPasswordService;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailMessageService = emailMessageService;
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            bool status = false;
            var result = await _signInManager.PasswordSignInAsync(EncryptionService.EncryptEmail(model.Email), model.Password, model.RememberMe, shouldLockout: true);
            switch (result)
            {
                case SignInStatus.Success:
                    TwoFactorAuthenticator TwoFacAuth = new TwoFactorAuthenticator();
                    //string UserUniqueKey = (model.Email + EwiPracaConstants.GoogleKeys.TwoFactorAuthenticatorKey);
                    //Session["UserUniqueKey"] = UserUniqueKey;
                    ////var setupInfo = TwoFacAuth.GenerateSetupCode("EwiPraca", model.Email, UserUniqueKey, 300, 300);
                    ////ViewBag.BarcodeImageUrl = setupInfo.QrCodeSetupImageUrl;
                    ////ViewBag.SetupCode = setupInfo.ManualEntryKey;
                    //status = true;
                    //ViewBag.Status = status;
                    //return View();
                    return RedirectToAction("Index", "Home");

                case SignInStatus.LockedOut:
                    return View("Lockout");

                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });

                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Błędne hasło lub adres email.");
                    ViewBag.Status = status;
                    return View(model);
            }
        }

        public ActionResult TwoFactorAuthenticate(string CodeDigit)
        {
            var token = CodeDigit;
            TwoFactorAuthenticator TwoFacAuth = new TwoFactorAuthenticator();
            string UserUniqueKey = Session["UserUniqueKey"].ToString();
            bool isValid = TwoFacAuth.ValidateTwoFactorPIN(UserUniqueKey, token);

            if (isValid)
            {
                Session["IsValidTwoFactorAuthentication"] = true;
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Login", "Home");
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = new ApplicationUser
                    {
                        UserName = EncryptionService.EncryptEmail(model.Email),
                        Email = EncryptionService.EncryptEmail(model.Email),
                        FirstName = EncryptionService.Encrypt(model.FirstName),
                        Surname = EncryptionService.Encrypt(model.LastName),
                        LastLoginDate = DateTime.Now
                    };
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                        // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                        // Send an email with this link
                        // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                        // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                        return RedirectToAction("Index", "Home");
                    }
                    AddErrors(result);
                }
                catch (Exception e)
                {
                    logger.Error(e, e.Message);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(EncryptionService.EncryptEmail(model.Email));
                if (user == null)// || !(await _userManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                try
                {
                    string guid = Guid.NewGuid().ToString();

                    int hoursValidLink = SettingsHandler.PasswordResetEmailValidDuration;
                    var validTo = DateTime.Now.AddHours(hoursValidLink);

                    ResetPasswordRequest request = new ResetPasswordRequest()
                    {
                        ApplicationUserID = user.Id,
                        ResetGuid = guid,
                        ValidTo = validTo
                    };

                    _resetPasswordService.Create(request);

                    var message = CreateForgottenPasswordMessage(user.Id, guid, validTo, model.Email);

                    _emailMessageService.SendEmailMessage(message);

                    request.MailSentDate = message.SentDate;

                    _resetPasswordService.Update(request);
                }
                catch(Exception e)
                {
                    logger.Error(e, e.Message);
                }


                return View("ForgotPasswordConfirmation");

            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private EmailMessage CreateForgottenPasswordMessage(string id, string guid, DateTime validTo, string email)
        {
            string link = Url.Action("ResetPassword", "Account", new { guid = guid }, Request.Url.Scheme);

            EmailMessage message = new EmailMessage()
            {
                ApplicationUserID = id,
                EmailType = Enumerations.EmailType.PasswordResetRequest,
                Recipient = email,
                Body = WebResources.PasswordResetEmailBody.Replace("XXXLINK", link).Replace("XXXDATE", validTo.ToShortDateString()),
                Subject = WebResources.PasswordResetEmailTitle
            };

            _emailMessageService.Create(message);

            return message;
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string guid)
        {
            var resetRequest = _resetPasswordService.GetByGuid(guid);

            if(resetRequest == null || resetRequest.ValidTo < DateTime.Now || resetRequest.IsCompleted)
            {
                return View("ResetPasswordError");
            }

            ResetPasswordViewModel model = new ResetPasswordViewModel()
            {
                Guid = resetRequest.ResetGuid,
                Email = resetRequest.ApplicationUser.Email
            };

            return View(model);
        }

       [HttpPost]
       [AllowAnonymous]
       [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }

            try
            {
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user.Id);
                var result = await _userManager.ResetPasswordAsync(user.Id, resetToken, model.Password);

                if (result.Succeeded)
                {
                    var resetPasswordRequest = _resetPasswordService.GetByGuid(model.Guid);
                    resetPasswordRequest.IsCompleted = true;
                    _resetPasswordService.Update(resetPasswordRequest);

                    return RedirectToAction("ResetPasswordConfirmation", "Account");
                }

                AddErrors(result);
            }
            catch(Exception e)
            {
                logger.Error(e, e.Message);
            }

            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        #region Helpers

        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        #endregion Helpers
    }
}