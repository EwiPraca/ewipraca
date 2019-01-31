using EwiPraca.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EwiPraca.Controllers
{
    [CookieConsent]
    public class ErrorController : Controller
    {
        public ActionResult ErrorPage(Exception exception)
        {
            return View(exception);
        }

        public ActionResult Unauthorized()
        {
            return View();
        }
    }
}