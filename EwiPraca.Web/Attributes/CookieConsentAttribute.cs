using System;
using System.Web;
using System.Web.Mvc;

namespace EwiPraca.Attributes
{
    /// <summary>
    /// ASP.NET MVC FilterAttribute for implementing european cookie-law
    /// </summary>
    public class CookieConsentAttribute : ActionFilterAttribute
    {
        public const string CONSENT_COOKIE_NAME = "CookieEwiPracaConsent";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var viewBag = filterContext.Controller.ViewBag;
            viewBag.AskCookieConsent = true;
            viewBag.HasCookieConsent = false;

            var request = filterContext.HttpContext.Request;

            // Check if the user has a consent cookie
            var consentCookie = request.Cookies[CONSENT_COOKIE_NAME];

            if (consentCookie == null)
            {
                // No consent cookie. We first check the Do Not Track header value, this can have the value "0" or "1"
                string dnt = request.Headers.Get("DNT");

                // If we receive a DNT header, we accept its value and do not ask the user anymore
                if (!String.IsNullOrEmpty(dnt))
                {
                    viewBag.AskCookieConsent = false;
                    if (dnt == "0")
                    {
                        viewBag.HasCookieConsent = true;
                    }
                }
                else
                {
                    if (IsSearchCrawler(request.Headers.Get("User-Agent")))
                    {
                        // don't ask consent from search engines, also don't set cookies
                        viewBag.AskCookieConsent = false;
                    }
                    else
                    {
                        // first request on the site and no DNT header.
                        consentCookie = new HttpCookie(CONSENT_COOKIE_NAME);
                        consentCookie.Value = "asked";
                        filterContext.HttpContext.Response.Cookies.Add(consentCookie);
                    }
                }
            }
            else
            {
                // we received a consent cookie
                viewBag.AskCookieConsent = false;
                if (consentCookie.Value == "asked")
                {
                    // consent is implicitly given
                    consentCookie.Value = "true";
                    consentCookie.Expires = DateTime.UtcNow.AddYears(1);
                    filterContext.HttpContext.Response.Cookies.Set(consentCookie);
                    viewBag.HasCookieConsent = true;
                }
                else if (consentCookie.Value == "true")
                {
                    viewBag.HasCookieConsent = true;
                }
                else
                {
                    // assume consent denied
                    viewBag.HasCookieConsent = false;
                }
            }
            base.OnActionExecuting(filterContext);
        }

        private bool IsSearchCrawler(string userAgent)
        {
            if (!string.IsNullOrEmpty(userAgent))
            {
                string[] crawlers = new string[]
                {
                    "Baiduspider",
                    "Googlebot",
                    "YandexBot",
                    "YandexImages",
                    "bingbot",
                    "msnbot",
                    "Vagabondo",
                    "SeznamBot",
                    "ia_archiver",
                    "AcoonBot",
                    "Yahoo! Slurp",
                    "AhrefsBot"
                };

                foreach (string crawler in crawlers)
                    if (userAgent.Contains(crawler))
                        return true;
            }
            return false;
        }
    }


    /// <summary>
    /// Helper class for easy/typesafe getting the cookie consent status
    /// </summary>
    public static class CookieConsent
    {
        public static void SetCookieConsent(HttpResponseBase response, bool consent)
        {
            var consentCookie = new HttpCookie(CookieConsentAttribute.CONSENT_COOKIE_NAME);
            consentCookie.Value = consent ? "true" : "false";
            consentCookie.Expires = DateTime.UtcNow.AddYears(1);
            response.Cookies.Set(consentCookie);
        }

        public static bool AskCookieConsent(ViewContext context)
        {
            return context.ViewBag.AskCookieConsent ?? false;
        }

        public static bool HasCookieConsent(ViewContext context)
        {
            return context.ViewBag.HasCookieConsent ?? false;
        }
    }
}