using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EwiPraca.Controllers
{
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