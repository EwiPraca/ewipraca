using EwiPraca.Attributes;
using System.Web.Mvc;

namespace EwiPraca.Controllers
{
    [AllowAnonymous]
    public class SiteController : Controller
    {
        public ActionResult AllowCookies(string ReturnUrl)
        {
            CookieConsent.SetCookieConsent(Response, true);

            return Redirect(ReturnUrl);
        }

        public ActionResult NoCookies(string ReturnUrl)
        {
            CookieConsent.SetCookieConsent(Response, false);

            if (Request.IsAjaxRequest())
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
            else
                return Redirect(ReturnUrl);
        }
    }
}