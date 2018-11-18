using EwiPraca.App_Start;
using FluentValidation.Mvc;
using NLog;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace EwiPraca
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            UnityConfig.RegisterComponents();
            EwiClassMapper.Setup();
            FluentValidationModelValidatorProvider.Configure();
            ViewEngines.Engines.Add(new EwiPracaViewEngine());
        }

        protected void Application_Error()
        {
            var ex = Server.GetLastError();
            
            if(ex != null)
            {
                logger.Error(ex, ex.Message);
            }
        }
    }
}