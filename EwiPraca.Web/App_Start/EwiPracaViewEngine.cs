using System.Web.Mvc;

namespace EwiPraca.App_Start
{
    public class EwiPracaViewEngine : WebFormViewEngine
    {
        public EwiPracaViewEngine()
        {
            var viewLocations = new[] {
            "~/Views/{1}/{0}.cshtml"
            };

            PartialViewLocationFormats = viewLocations;
            ViewLocationFormats = viewLocations;
        }
    }
}