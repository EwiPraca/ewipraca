using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EwiPraca.Startup))]
namespace EwiPraca
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
